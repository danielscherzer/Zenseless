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
#if SOLUTION
			texDiffuse = contentLoader.Load<ITexture2D>("capsule0");
#endif
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
#if SOLUTION
			texDiffuse.Activate();
#endif
			geometry.Draw();
#if SOLUTION
			texDiffuse.Deactivate();
#endif
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private IDrawable geometry;
#if SOLUTION
		private ITexture texDiffuse;
#endif
	}
}
