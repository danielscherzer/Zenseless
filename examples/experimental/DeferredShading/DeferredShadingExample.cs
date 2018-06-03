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

			var camera = window.GameWindow.CreateOrbitingCameraController(0.3f, 70, 0.01f, 30f);
			camera.View.Elevation = 15;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			window.Render += () => visual.Draw(camera);
			window.Resize += visual.Resize;
			window.Run();

		}
	}
}