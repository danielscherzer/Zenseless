namespace Example
{
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var orbit = window.GameWindow.CreateOrbitingCameraController(1.5f, 90f, 0.1f, 50f);
			orbit.Azimuth = 90;
			orbit.Elevation = 20;
			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			window.Render += () => visual.Render(orbit);
			window.Run();

		}
	}
}
