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
			var window = new ExampleWindow(debug:true);
			var camera = window.GameWindow.CreateOrbitingCameraController(3f, 70f, 0.1f, 500f);
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += () => visual.Render(camera, camera.View.CalcPosition());
			window.Run();

		}
	}
}