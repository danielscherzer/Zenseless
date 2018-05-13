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
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var camera = window.GameWindow.CreateOrbitingCameraController(3, 70, 0.1f, 20f);
			camera.View.Elevation = 15;

			window.Render += () => visual.Render(camera);
			window.Run();

		}
	}
}