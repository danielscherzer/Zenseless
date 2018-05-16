using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Zenseless.ExampleFramework
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IDrawConfiguration" />
	public class DrawConfiguration : IDrawConfiguration
	{
		/// <summary>
		/// Gets or sets a value indicating whether [backface culling].
		/// </summary>
		/// <value>
		///   <c>true</c> if [backface culling]; otherwise, <c>false</c>.
		/// </value>
		public bool BackfaceCulling { get; set; } = false;
		/// <summary>
		/// Gets or sets the instance count.
		/// </summary>
		/// <value>
		/// The instance count.
		/// </value>
		public int InstanceCount { get; set; } = 1;
		/// <summary>
		/// Gets or sets a value indicating whether [shader point size].
		/// </summary>
		/// <value>
		///   <c>true</c> if [shader point size]; otherwise, <c>false</c>.
		/// </value>
		public bool ShaderPointSize { get; set; } = false;
		/// <summary>
		/// Gets the shader.
		/// </summary>
		/// <value>
		/// The shader.
		/// </value>
		public IShaderProgram ShaderProgram { get; private set; }
		/// <summary>
		/// Gets the vao.
		/// </summary>
		/// <value>
		/// The vao.
		/// </value>
		public VAO Vao { get; private set; }
		/// <summary>
		/// Gets or sets a value indicating whether [z buffer test].
		/// </summary>
		/// <value>
		///   <c>true</c> if [z buffer test]; otherwise, <c>false</c>.
		/// </value>
		public bool ZBufferTest { get; set; } = false;

		/// <summary>
		/// Draws the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Draw(IRenderContext context)
		{
			context.RenderState.Set(new ShaderPointSize(ShaderPointSize));
			context.RenderState.Set(new DepthTest(ZBufferTest));
			context.RenderState.Set(new BackFaceCulling(BackfaceCulling));
			context.RenderState.Set(new ActiveShader(ShaderProgram));

			BindTextures();
			ActivateBuffers();

			var vao = Vao;
			if (vao is null)
			{
				if (1 == InstanceCount)
				{
					GL.DrawArrays(PrimitiveType.Quads, 0, 4); //TODO: make this general -> mesh with only vertex count? particle system, sprites
				}
				else
				{
					context.DrawPoints(InstanceCount);
				}
			}
			else
			{
				Vao.Draw(InstanceCount);
			}

			DeactivateBuffers();
			UnbindTextures();
		}

		/// <summary>
		/// Sets the input texture.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="texture">The texture.</param>
		public void SetInputTexture(string name, ITexture texture)
		{
			textures[name] = texture;
		}

		/// <summary>
		/// Sets the input texture.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="image">The image.</param>
		public void SetInputTexture(string name, IOldRenderSurface image)
		{
			textures[name] = image.Texture;
		}

		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		public void UpdateInstanceAttribute(string name, int[] data)
		{
			Vao.SetAttribute(GetAttributeShaderLocationAndCheckVao(name), data, VertexAttribPointerType.Int, 1, true);
		}

		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		public void UpdateInstanceAttribute(string name, float[] data)
		{
			Vao.SetAttribute(GetAttributeShaderLocationAndCheckVao(name), data, VertexAttribPointerType.Float, 1, true);
		}

		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		public void UpdateInstanceAttribute(string name, System.Numerics.Vector2[] data)
		{
			Vao.SetAttribute(GetAttributeShaderLocationAndCheckVao(name), data, VertexAttribPointerType.Float, 2, true);
		}

		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		public void UpdateInstanceAttribute(string name, System.Numerics.Vector3[] data)
		{
			Vao.SetAttribute(GetAttributeShaderLocationAndCheckVao(name), data, VertexAttribPointerType.Float, 3, true);
		}

		/// <summary>
		/// Updates the instance attribute.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="data">The data.</param>
		public void UpdateInstanceAttribute(string name, System.Numerics.Vector4[] data)
		{
			Vao.SetAttribute(GetAttributeShaderLocationAndCheckVao(name), data, VertexAttribPointerType.Float, 4, true);
		}

		//public void UpdateInstanceAttribute<DATA_ELEMENT_TYPE>(string name, DATA_ELEMENT_TYPE[] data) where DATA_ELEMENT_TYPE : struct
		//{
		//	Vao.SetAttribute(GetAttributeShaderLocationAndCheckVao(name), data, VertexAttribPointerType.Float, 3, true);
		//}

		/// <summary>
		/// Updates the mesh shader.
		/// </summary>
		/// <param name="mesh">The mesh.</param>
		/// <param name="shaderProgram">The shader program.</param>
		/// <exception cref="ArgumentNullException">
		/// mesh
		/// or
		/// shaderProgram
		/// or
		/// A shaderName is required
		/// </exception>
		/// <exception cref="ArgumentException">Shader '" + shaderName + "' does not exist</exception>
		public void UpdateMeshShader(DefaultMesh mesh, IShaderProgram shaderProgram)
		{
			ShaderProgram = shaderProgram ?? throw new ArgumentNullException(nameof(shaderProgram));
			Vao = mesh is null ? null : VAOLoader.FromMesh(mesh, ShaderProgram);
		}
		
		/// <summary>
				/// Updates the uniforms.
				/// </summary>
				/// <typeparam name="DATA">The type of the ata.</typeparam>
				/// <param name="name">The name.</param>
				/// <param name="uniforms">The uniforms.</param>
		public void UpdateUniforms<DATA>(string name, DATA uniforms) where DATA : struct
		{
			if (!buffers.TryGetValue(name, out BufferObject buffer))
			{
				buffer = new BufferObject(BufferTarget.UniformBuffer);
				buffers.Add(name, buffer);
			}
			buffer.Set(uniforms, BufferUsageHint.StaticRead);
		}

		/// <summary>
		/// Updates the shader buffer.
		/// </summary>
		/// <typeparam name="DATA_ELEMENT_TYPE">The type of the ata element type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="uniformArray">The uniform array.</param>
		public void UpdateShaderBuffer<DATA_ELEMENT_TYPE>(string name, DATA_ELEMENT_TYPE[] uniformArray) where DATA_ELEMENT_TYPE : struct
		{
			if (!buffers.TryGetValue(name, out BufferObject buffer))
			{
				buffer = new BufferObject(BufferTarget.ShaderStorageBuffer);
				buffers.Add(name, buffer);
			}
			buffer.Set(uniformArray, BufferUsageHint.StaticCopy);
		}

		/// <summary>
		/// The textures
		/// </summary>
		private readonly Dictionary<string, ITexture> textures = new Dictionary<string, ITexture>();
		/// <summary>
		/// The buffers
		/// </summary>
		private Dictionary<string, BufferObject> buffers = new Dictionary<string, BufferObject>();

		/// <summary>
		/// Activates the buffers.
		/// </summary>
		/// <exception cref="ArgumentException">Could not find shader parameters '" + uBuffer.Key + "'</exception>
		private void ActivateBuffers()
		{
			foreach (var uBuffer in buffers)
			{
				var bindingIndex = ShaderProgram.GetResourceLocation(uBuffer.Value.Type, uBuffer.Key);
				if (-1 == bindingIndex) throw new ArgumentException("Could not find shader parameters '" + uBuffer.Key + "'");
				uBuffer.Value.ActivateBind(bindingIndex);
			}
		}

		/// <summary>
		/// Deactivates the buffers.
		/// </summary>
		private void DeactivateBuffers()
		{
			foreach (var uBuffer in buffers)
			{
				uBuffer.Value.Deactivate();
			}
		}

		/// <summary>
		/// Binds the textures.
		/// </summary>
		private void BindTextures()
		{
			int id = 0;
			if (ShaderProgram is null)
			{
				foreach (var namedTex in textures)
				{
					GL.ActiveTexture(TextureUnit.Texture0 + id);
					namedTex.Value.Activate();
					++id;
				}
			}
			else
			{
				foreach (var namedTex in textures)
				{
					GL.ActiveTexture(TextureUnit.Texture0 + id);
					namedTex.Value.Activate();
					ShaderProgram.Uniform(namedTex.Key, id);
					++id;
				}
			}
		}

		/// <summary>
		/// Unbinds the textures.
		/// </summary>
		private void UnbindTextures()
		{
			int id = 0;
			foreach (var namedTex in textures)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				namedTex.Value.Deactivate();
				++id;
			}
			GL.ActiveTexture(TextureUnit.Texture0);
		}

		/// <summary>
		/// Gets the attribute shader location and check vao.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Specify mesh before setting instance attributes</exception>
		private int GetAttributeShaderLocationAndCheckVao(string name)
		{
			if (Vao is null) throw new InvalidOperationException("Specify mesh before setting instance attributes");
			return ShaderProgram.GetResourceLocation(ShaderResourceType.Attribute, name);
		}
	}
}