﻿using OpenTK.Graphics.OpenGL4;
using Zenseless.Base;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	public class TextureToFrameBuffer : Disposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentShader">The current shader.</param>
		public delegate void SetUniforms(IShaderProgram currentShader);

		/// <summary>
		/// Initializes a new instance of the <see cref="TextureToFrameBuffer"/> class.
		/// </summary>
		/// <param name="fragmentShader">The fragment shader.</param>
		/// <param name="vertexShader">The vertex shader.</param>
		public TextureToFrameBuffer(string fragmentShader = DefaultShader.FragmentShaderCopy, string vertexShader = DefaultShader.VertexShaderScreenQuad)
		{
			shaderProgram = ShaderLoader.FromStrings(vertexShader, fragmentShader);
		}

		/// <summary>
		/// Draws the specified texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="setUniformsHandler">The set uniforms handler.</param>
		public void Draw(ITexture texture, SetUniforms setUniformsHandler = null)
		{
			shaderProgram.Activate();
			texture.Activate();
			setUniformsHandler?.Invoke(shaderProgram);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			texture.Deactivate();
			shaderProgram.Deactivate();
		}

		/// <summary>
		/// The shader
		/// </summary>
		private IShaderProgram shaderProgram;

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			shaderProgram.Dispose();
		}
	}
}
