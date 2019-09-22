namespace Zenseless.OpenGL
{
	using GLSLhelper;
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// Creates a content manager that creates OpenGL object instances, like textures, shaders
	/// </summary>
	public static class ContentManagerGL
	{
		/// <summary>
		/// Creates a content manager that creates OpenGL object instances, like textures
		/// </summary>
		/// <param name="resourceAssembly">The assembly that contains the resources.</param>
		/// <param name="solutionMode">Should shaders be built with solution on or off</param>
		/// <returns>A content manager instance</returns>
		public static FileContentManager Create(Assembly resourceAssembly, bool solutionMode)
		{
			var streamLoaderCollection = new NamedStreamLoaderCollection();
			//streamLoaderCollection.Add(new NamedFileStreamLoader()); //load arbitrary files TODO: file name resolution
			streamLoaderCollection.Add(new NamedResourceStreamLoader(resourceAssembly));
			streamLoaderCollection.Add(new NamedResourceStreamLoader(Assembly.GetExecutingAssembly())); //Zenseless.OpenGL resources

			var mgr = new FileContentManager(streamLoaderCollection);
			mgr.RegisterImporter((res) => ContentImporters.StringBuilder(res).ToString());
			mgr.RegisterImporter(ContentImporters.StringBuilder);
			mgr.RegisterImporter<Stream>((it) => 
				{
					var input = it.First().Stream;
					var s = new MemoryStream();
					it.First().Stream.CopyTo(s);
					s.Position = 0;
					return s;
				}
			);
			mgr.RegisterUpdater<StringBuilder>(ContentImporters.Update);
			mgr.RegisterImporter(ContentImporters.ByteBuffer);
			mgr.RegisterImporter(ContentImporters.DefaultMesh);
			mgr.RegisterImporter(BitmapImporter);
			mgr.RegisterUpdater<Texture2dGL>(Update);
			mgr.RegisterImporter(TextureArrayImporter);
			mgr.RegisterUpdater<TextureArray2dGL>(Update);
			mgr.RegisterImporter((namedStreams) => ShaderProgramImporter(namedStreams, solutionMode, mgr));
			mgr.RegisterUpdater<ShaderProgramGL>((prog, namedStreams) => Update(prog, namedStreams, solutionMode, mgr));
			return mgr;
		}


		/// <summary>
		/// Gets the shader type from file extension.
		/// </summary>
		/// <param name="extension">The file extension.</param>
		/// <returns>the shader type</returns>
		/// <exception cref="ArgumentException"></exception>
		public static HLGL.ShaderType GetShaderTypeFromExtension(string extension)
		{
			if(mapExtensionToShaderType.TryGetValue(extension.ToLowerInvariant(), out var type))
			{
				return type;
			}
			throw new ArgumentException($"File extension {extension} is not valid for a shader.");
		}

		private static ReadOnlyDictionary<string, HLGL.ShaderType> mapExtensionToShaderType = 
			new ReadOnlyDictionary<string, HLGL.ShaderType>(new Dictionary<string, HLGL.ShaderType>
		{
			{ ".comp" , HLGL.ShaderType.ComputeShader },
			{ ".frag" , HLGL.ShaderType.FragmentShader },
			{ ".glsl" , HLGL.ShaderType.FragmentShader },
			{ ".geom" , HLGL.ShaderType.GeometryShader },
			{ ".tesc" , HLGL.ShaderType.TessControlShader },
			{ ".tese" , HLGL.ShaderType.TessEvaluationShader },
			{ ".vert" , HLGL.ShaderType.VertexShader },
		});

		private static void Update(Texture2dGL texture, IEnumerable<NamedStream> resources)
		{
			foreach (var res in resources)
			{
				using (var bitmap = new Bitmap(res.Stream))
				{
					texture.FromBitmap(bitmap);
					return;
				}
				//TODO: if multiple textures assume these contain mipmap levels and load them
			}
		}

		private static ITexture2D BitmapImporter(IEnumerable<NamedStream> resources)
		{
			var texture = new Texture2dGL();
			Update(texture, resources);
			return texture;
		}

		private static IShaderProgram ShaderProgramImporter(IEnumerable<NamedStream> resources, bool solutionMode, IContentLoader contentLoader)
		{
			ShaderProgramGL shaderProgram = new ShaderProgramGL();
			Update(shaderProgram, resources, solutionMode, contentLoader);
			return shaderProgram;
		}

		private static void Update(ShaderProgramGL shaderProgram, IEnumerable<NamedStream> namedStreams, bool solutionMode, IContentLoader contentLoader)
		{
			string ShaderCode(Stream stream)
			{
				using (var reader = new StreamReader(stream, true))
				{
					var code = reader.ReadToEnd();
					if (solutionMode) code = code.Replace("#ifdef SOLUTION", "#if 1");
					return code;
				}
			}

			var count = namedStreams.Count();
			if (2 > count) return;
			foreach (var res in namedStreams)
			{
				var shaderType = GetShaderTypeFromExtension(Path.GetExtension(res.Name));
				var shaderCode = ShaderCode(res.Stream);
				string GetIncludeCode(string includeName)
				{
					var resourceName = includeName.Replace(Path.DirectorySeparatorChar, '.');
					resourceName = resourceName.Replace(Path.AltDirectorySeparatorChar, '.');
					var includeCode = contentLoader.Load<StringBuilder>(resourceName).ToString();
					using (var includeShader = new ShaderGL(shaderType))
					{
						if (!includeShader.Compile(includeCode))
						{
							var e = includeShader.CreateException(includeCode);
							throw new NamedShaderException(contentLoader.Names.GetFullName(includeName), e);
						}
					}
					return includeCode;
				}
				var expandedShaderCode = Transformations.ExpandIncludes(shaderCode, GetIncludeCode);
				var shader = new ShaderGL(shaderType);
				if (shader.Compile(expandedShaderCode))
				{
					shaderProgram.Attach(shader);
				}
				else
				{
					shaderProgram.RemoveShaders(); //clean out whole shader program to avoid double attachments next time
					var e = shader.CreateException(shaderCode);
					shader.Dispose();
					throw new NamedShaderException(res.Name, e);
				}
			}
			if(!shaderProgram.Link())
			{
				var e = new ShaderLinkException(shaderProgram.Log);
				var s = new StringBuilder();
				foreach (var ns in namedStreams)
				{
					s.Append(ns.Name);
					s.Append(';');
				}
				throw new NamedShaderException(s.ToString(), e);
			}
		}

		private static ITexture2dArray TextureArrayImporter(IEnumerable<NamedStream> resources)
		{
			var texArray = new TextureArray2dGL();
			Update(texArray, resources);
			return texArray;
		}

		private static void Update(TextureArray2dGL texArray, IEnumerable<NamedStream> resources)
		{
			var count = resources.Count();
			if (2 > count) return;

			var bitmaps = from res in resources select new Bitmap(res.Stream);

			var first = bitmaps.First();
			var levels = MathHelper.MipMapLevels(first.Width, first.Height);
			var internalFormat = TextureLoaderDrawing.SelectInternalPixelFormat(first.PixelFormat);

			texArray.SetFormat(first.Width, first.Height, count, levels, (SizedInternalFormat)internalFormat);
			var slice = 0;
			foreach (var bitmap in bitmaps)
			{
				var buffer = bitmap.ToBuffer();
				var pixelFormat = TextureLoaderDrawing.SelectPixelFormat(bitmap.PixelFormat);
				texArray.Load(buffer, slice, pixelFormat, PixelType.UnsignedByte);
				++slice;
			}
			texArray.Filter = TextureFilterMode.Mipmap;
			texArray.WrapFunction = TextureWrapFunction.ClampToEdge;
		}
		//private static VAO VAOImporter(IEnumerable<NamedStream> resources)
		//{
		//	Obj2Mesh.FromObj()
		//	var texArray = new TextureArray2dGL();
		//	Update(texArray, resources);
		//	return texArray;
		//}
	}
}
