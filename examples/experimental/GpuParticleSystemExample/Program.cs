namespace Example
{
	using System;
	using Zenseless.Patterns;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Application
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var camera = window.GameWindow.CreateOrbitingCameraController(2, 70, 0.1f, 20f);
			camera.View.Elevation = 15;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => visual.Render(time.DeltaTime, camera);
			window.Resize += visual.Resize;
			window.Run();

		}
	}
}