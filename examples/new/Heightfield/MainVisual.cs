using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Heightfield
{
	internal class MainVisual
	{
		public MainVisual(IRenderContext renderContext, IContentLoader contentLoader)
		{
			renderContext.RenderState.Set(BoolState<IDepthState>.Enabled);
			framebuffer = renderContext.GetFrameBuffer();
			shader = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = Meshes.CreatePlane(50f, 50f, 50, 50);
			geometry = VAOLoader.FromMesh(mesh, shader);
		}

		internal void Render()
		{
			framebuffer.Clear();
			shader.Activate();
			geometry.Draw();
			shader.Deactivate();
		}

		private readonly IRenderSurface framebuffer;
		private readonly IShaderProgram shader;
		private readonly VAO geometry;
	}
}