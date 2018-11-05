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
			var camera = window.GameWindow.CreateOrbitingCameraController(15, 30, 0.1f, 50f);
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += () => visual.Render(camera, camera.View.CalcPosition());
			window.Run();

		}
	}
}