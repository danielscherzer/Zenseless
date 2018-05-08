namespace Example
{
	using System;
	using Zenseless.Base;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var controller = new Controller();

			var camera = window.GameWindow.CreateOrbitingCameraController(2, 70, 0.1f, 20f);
			camera.Elevation = 15;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var time = new GameTime();
			window.Render += () => visual.Render(camera);
			window.Update += (t) => visual.Update(time.AbsoluteTime);
			window.Run();
			window.Dispose();
		}
	}
}