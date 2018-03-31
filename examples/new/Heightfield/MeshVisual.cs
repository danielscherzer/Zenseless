using System;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Heightfield
{
	public class MeshVisual
	{
		public MeshVisual(DefaultMesh mesh, IShaderProgram shader, TextureBinding[] textureBindings)
		{
			shaderProgram = shader;
			geometry = VAOLoader.FromMesh(mesh, shader);
			this.textureBindings = textureBindings;
		}

		public void Draw()
		{
			BindTextures();
			geometry.Draw();
			UnbindTextures();
		}

		private readonly IShaderProgram shaderProgram;
		private readonly VAO geometry;
		private TextureBinding[] textureBindings;

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
