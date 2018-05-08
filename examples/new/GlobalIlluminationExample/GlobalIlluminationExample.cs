namespace Example
{
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var orbit = window.GameWindow.CreateOrbitingCameraController(1.8f, 70, 0.1f, 50f);
			orbit.TargetY = -0.3f;

			window.Render += () => visual.Render(orbit, orbit.CalcPosition());
			window.Run();

		}
	}
}