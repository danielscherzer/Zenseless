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
			var camera = window.GameWindow.CreateOrbitingCameraController(1.5f, 90f, 0.1f, 50f);
			camera.View.Azimuth = 90;
			camera.View.Elevation = 20;
			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			window.Render += () => visual.Render(camera);
			window.Run();

		}
	}
}
