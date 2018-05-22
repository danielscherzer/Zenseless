using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.Patterns;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Disposable" />
	public class PostProcessing : Disposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentShader">The current shader.</param>
		public delegate void SetUniforms(IShaderProgram currentShader);

		/// <summary>
		/// Initializes a new instance of the <see cref="PostProcessing"/> class.
		/// </summary>
		public PostProcessing(IShaderProgram shaderProgram)
		{
			this.shaderProgram = shaderProgram ?? throw new ArgumentNullException(nameof(shaderProgram));
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
