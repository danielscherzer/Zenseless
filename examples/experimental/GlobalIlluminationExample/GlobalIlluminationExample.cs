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
			var camera = window.GameWindow.CreateOrbitingCameraController(1.8f, 70, 0.1f, 50f);
			camera.View.TargetY = -0.3f;

			window.Render += () => visual.Render(camera, camera.View.CalcPosition());
			window.Run();

		}
	}
}