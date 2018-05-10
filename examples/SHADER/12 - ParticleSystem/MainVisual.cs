namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new DepthTest(true));
			plane = new VisualPlane(renderState, contentLoader);

			visualSmoke = new VisualSmoke(Vector3.Zero, new Vector3(.2f, 0, 0), renderState, contentLoader);
			visualWaterfall = new VisualWaterfall(new Vector3(-.5f, 1, -.5f), renderState, contentLoader);
		}

		public void Update(float time)
		{
			visualSmoke.Update(time);
			visualWaterfall.Update(time);
		}

		public void Render(Transformation3D camera)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var cam = camera.CalcLocalToWorldColumnMajorMatrix();
			plane.Draw(cam);
			visualSmoke.Render(cam);
			visualWaterfall.Render(cam);
		}

		internal void Resize(int width, int height)
		{
			visualSmoke.Resize(width, height);
			visualWaterfall.Resize(width, height);
		}

		private VisualPlane plane;
		private readonly VisualSmoke visualSmoke;
		private readonly VisualWaterfall visualWaterfall;
	}
}
