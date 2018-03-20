using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Heightfield
{
	internal class MainVisual
	{
		public MainVisual(IRenderContext renderContext, IContentLoader contentLoader)
		{
			Camera.FarClip = 20;
			Camera.Distance = 2;
			Camera.FovY = 70;
			Camera.Elevation = 15;

			renderContext.RenderState.Set(BoolState<IDepthState>.Enabled);
			renderContext.RenderState.Set(BoolState<IBackfaceCullingState>.Disabled);

			shader = contentLoader.Load<IShaderProgram>("shader.*");
			heightfield = contentLoader.Load<ITexture2D>("mountain_height");
			color = contentLoader.Load<ITexture2D>("mountain_color");
			var mesh = Meshes.CreatePlane(2, 2, 1024, 1024);
			geometry = VAOLoader.FromMesh(mesh, shader);
		}

		public CameraOrbit Camera { get; private set; } = new CameraOrbit();

		internal void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.ActiveTexture(TextureUnit.Texture0);
			heightfield.Activate();
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "texHeightfield"), 0);
			GL.ActiveTexture(TextureUnit.Texture1);
			color.Activate();
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "texColor"), 1);
			var mtxCamera = Camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref mtxCamera);
			geometry.Draw();
			color.Deactivate();
			GL.ActiveTexture(TextureUnit.Texture0);
			heightfield.Deactivate();
			shader.Deactivate();
		}

		private readonly IShaderProgram shader;
		private readonly VAO geometry;
		private readonly ITexture heightfield;
		private readonly ITexture color;
	}
}