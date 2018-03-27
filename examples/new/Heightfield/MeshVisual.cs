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
			this.shader = shader;
			geometry = VAOLoader.FromMesh(mesh, shader);
			this.textureBindings = textureBindings;
		}

		public void Draw(Action<Func<string, int>> uniformSetter)
		{
			shader.Activate();
			BindTextures();
			int GetLocation(string name) => shader.GetResourceLocation(ShaderResourceType.Uniform, name);

			uniformSetter?.Invoke(GetLocation);
			geometry.Draw();
			UnbindTextures();
			shader.Deactivate();
		}

		private IShaderProgram shader;
		private readonly VAO geometry;
		private TextureBinding[] textureBindings;

		private void BindTextures()
		{
			int id = 0;
			foreach (var binding in textureBindings)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + id);
				binding.Texture.Activate();
				shader.Uniform(binding.Name, id);
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
