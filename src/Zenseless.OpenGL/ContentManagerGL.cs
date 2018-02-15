using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Zenseless.Base;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Creates a content manager that creates OpenGL object instances, like textures
	/// </summary>
	public static class ContentManagerGL
	{
		/// <summary>
		/// Creates a content manager that creates OpenGL object instances, like textures
		/// </summary>
		/// <param name="resourceAssembly">The resource assembly that contains the resources.</param>
		/// <returns>A content manager instance</returns>
		public static IContentManager Create(Assembly resourceAssembly)
		{
			var mgr = new CachedContentManagerDecorator(new ResourceContentManager(resourceAssembly));
			mgr.RegisterImporter(ContentImporters.String);
			mgr.RegisterImporter(ContentImporters.ByteBuffer);
			mgr.RegisterImporter(BitmapConverter);
			mgr.RegisterImporter(TextureArrayConverter);
			mgr.RegisterImporter(ShaderProgramConverter);
			return mgr;
		}

		/// <summary>
		/// Gets the shader type from file extension.
		/// </summary>
		/// <param name="fileExtension">The file extension.</param>
		/// <returns>the shader type</returns>
		/// <exception cref="ArgumentException"></exception>
		public static HLGL.ShaderType GetShaderTypeFromFileExtension(string fileExtension)
		{
			if(mapFileExtensionToShaderType.TryGetValue(fileExtension.ToLowerInvariant(), out var type))
			{
				return type;
			}
			throw new ArgumentException($"File extension {fileExtension} is not valid for a shader.");
		}

		private static Dictionary<string, HLGL.ShaderType> mapFileExtensionToShaderType = new Dictionary<string, HLGL.ShaderType>
		{
			{ ".comp" , HLGL.ShaderType.ComputeShader },
			{ ".frag" , HLGL.ShaderType.FragmentShader },
			{ ".geom" , HLGL.ShaderType.GeometryShader },
			{ ".tesc" , HLGL.ShaderType.TessControlShader },
			{ ".tese" , HLGL.ShaderType.TessEvaluationShader },
			{ ".vert" , HLGL.ShaderType.VertexShader },
		};

		private static ITexture2D BitmapConverter(IEnumerable<NamedStream> resources)
		{
			foreach (var res in resources)
			{
				using (var bitmap = new Bitmap(res.Stream))
				{
					return TextureLoaderDrawing.FromBitmap(bitmap);
				}
				//TODO: load mipmap levels
			}
			return null;
		}

		private static IShaderProgram PixelShaderConverter(NamedStream res)
		{
			using (var reader = new StreamReader(res.Stream, true))
			{
				var shaderCode = reader.ReadToEnd();
				return ShaderLoader.FromStrings(DefaultShader.VertexShaderScreenQuad, shaderCode);
			}
		}

		private static IShaderProgram ShaderProgramConverter(IEnumerable<NamedStream> resources)
		{
			var count = resources.Count();
			if (0 == count) return null;
			if (1 == count) return PixelShaderConverter(resources.First());
			ShaderProgramGL shaderProgram = new ShaderProgramGL();
			try
			{
				foreach (var res in resources)
				{
					var shaderType = GetShaderTypeFromFileExtension(Path.GetExtension(res.Name));
					shaderProgram.Compile(ShaderCode(res.Stream), shaderType);
				}
				shaderProgram.Link();
			}
			catch
			{
				shaderProgram.Dispose();
				return null;
			}
			return shaderProgram;
		}

		private static string ShaderCode(Stream stream)
		{
			using (var reader = new StreamReader(stream, true))
			{
				return reader.ReadToEnd();
			}
		}

		private static ITexture2dArray TextureArrayConverter(IEnumerable<NamedStream> resources)
		{
			var count = resources.Count();
			if (2 > count) return null;

			var bitmaps = from res in resources select new Bitmap(res.Stream);

			var first = bitmaps.First();
			var levels = MathHelper.MipMapLevels(first.Width, first.Height);
			var internalFormat = TextureLoaderDrawing.SelectInternalPixelFormat(first.PixelFormat);

			var texArray = new TextureArray2dGL();
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
			return texArray;
		}
	}
}
