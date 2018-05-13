namespace Example
{
	using System;
	using Zenseless.Base;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Application
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var camera = window.GameWindow.CreateOrbitingCameraController(3, 70, 0.1f, 20);
			camera.View.Elevation = 35;
			camera.View.Azimuth = 60;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => window.GameWindow.Title = $"{visual.Render(time.DeltaTime, camera):F2}msec";
			window.Resize += visual.Resize;
			window.Run();

		}
	}
}