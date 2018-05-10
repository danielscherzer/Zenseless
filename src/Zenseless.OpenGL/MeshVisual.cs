namespace Zenseless.OpenGL
{
	using System;
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// 
	/// </summary>
	public class MeshVisual
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MeshVisual"/> class.
		/// </summary>
		/// <param name="mesh">The mesh.</param>
		/// <param name="shader">The shader.</param>
		/// <param name="textureBindings">The texture bindings.</param>
		public MeshVisual(DefaultMesh mesh, IShaderProgram shader, TextureBinding[] textureBindings)
		{
			shaderProgram = shader;
			geometry = VAOLoader.FromMesh(mesh, shader);
			this.textureBindings = textureBindings;
		}

		/// <summary>
		/// Draws this instance.
		/// </summary>
		public void Draw(Action<IShaderProgram> uniformSetter = null)
		{
			shaderProgram.Activate();
			BindTextures();
			uniformSetter?.Invoke(shaderProgram);
			geometry.Draw();
			UnbindTextures();
			shaderProgram.Deactivate();
		}

		private readonly IShaderProgram shaderProgram;
		private readonly VAO geometry;
		private readonly TextureBinding[] textureBindings;

		private void BindTextures()
		{
			int id = 0;
			foreach (var binding in textureBindings)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				binding.Texture.Activate();
				shaderProgram.Uniform(binding.Name, id);
				++id;
			}
		}

		private void UnbindTextures()
		{
			int id = 0;
			foreach (var binding in textureBindings)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				binding.Texture.Deactivate();
				++id;
			}
			GL.ActiveTexture(TextureUnit.Texture0);
		}
	}
}
