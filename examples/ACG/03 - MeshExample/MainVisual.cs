using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new DepthTest(true));
			renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			//texDiffuse = contentLoader.Load<ITexture2D>("capsule0");
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			//load geometry
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
//			texDiffuse.Activate();
			geometry.Draw();
//			texDiffuse.Deactivate();
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private IDrawable geometry;
//		private ITexture texDiffuse;
	}
}
