using OpenTK.Graphics.OpenGL;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace MiniGalaxyBirds
{
	internal class View
	{
		private readonly Renderer renderer;

		public View(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(BlendStates.AlphaBlend);

			renderer = new Renderer();
			renderer.RegisterFont(new TextureFont(contentLoader.Load<ITexture2D>("Video Phreak"), 10, 32));
			renderer.Register("player", contentLoader.Load<ITexture2D>("blueships1"));
			renderer.Register("enemy", contentLoader.Load<ITexture2D>("redship4"));
			renderer.Register("bulletPlayer", contentLoader.Load<ITexture2D>("blueLaserRay"));
			renderer.Register("bulletEnemy", contentLoader.Load<ITexture2D>("redLaserRay"));
			renderer.Register("explosion", contentLoader.Load<ITexture2D>("explosion"));
			renderer.Register("background", contentLoader.Load<ITexture2D>("background"));
			renderer.CreateDrawable("background", GameLogic.visibleFrame);
		}

		public IRenderer Renderer => renderer;

		public void DrawScreen(IReadOnlyBox2D clipFrame, uint points)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			if (!(clipFrame is null))
			{
				foreach (IDrawable drawable in renderer.Drawables)
				{
					if (clipFrame.Intersects(drawable.Rect))
					{
						drawable.Draw();
					}
				}
			}
			else
			{
				foreach (IDrawable drawable in renderer.Drawables)
				{
					drawable.Draw();
				}
			}
			renderer.Print(minX, 0f, 0f, .04f, points.ToString());
		}

		public void ResizeWindow(int width, int height)
		{
			float deltaX = 0.5f * ((width / (float)height) - 1.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			minX = -deltaX;
			GL.Ortho(minX, 1.0 + deltaX, 0.0, 1.0, 0.0, 1.0);
			GL.MatrixMode(MatrixMode.Modelview);
		}

		private float minX;
	}
}