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
			var camera = window.GameWindow.CreateOrbitingCameraController(8, 90, 0.1f, 50f);
			camera.View.Elevation = 30;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += () => visual.Render(camera);
			window.Run();

		}
	}
}