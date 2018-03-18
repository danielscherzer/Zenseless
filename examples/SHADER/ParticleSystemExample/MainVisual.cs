using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(BoolState<IDepthState>.Enabled);
			plane = new VisualPlane(renderState, contentLoader);

			visualSmoke = new VisualSmoke(Vector3.Zero, new Vector3(.2f, 0, 0), renderState, contentLoader);
			visualWaterfall = new VisualWaterfall(new Vector3(-.5f, 1, -.5f), renderState, contentLoader);

			camera.FarClip = 20;
			camera.Distance = 2;
			camera.FovY = 70;
			camera.Elevation = 15;
		}

		public void Update(float time)
		{
			visualSmoke.Update(time);
			visualWaterfall.Update(time);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var cam = camera.CalcMatrix().ToOpenTK();
			plane.Draw(cam);
			visualSmoke.Render(cam);
			visualWaterfall.Render(cam);
		}

		private CameraOrbit camera = new CameraOrbit();
		private VisualPlane plane;
		private readonly VisualSmoke visualSmoke;
		private readonly VisualWaterfall visualWaterfall;
	}
}
