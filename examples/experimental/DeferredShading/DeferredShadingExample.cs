namespace Example
{
	using OpenTK.Input;
	using System;
	using Zenseless.Patterns;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();

			var camera = window.GameWindow.CreateOrbitingCameraController(1.8f, 70, 0.1f, 50f);
			camera.View.Elevation = 15;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			window.Render += () => visual.Draw(camera);
			window.Resize += visual.Resize;
			window.Run();

		}
	}
}