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
			var orbit = window.GameWindow.CreateOrbitingCameraController(3, 70, 0.1f, 20);
			orbit.Elevation = 35;
			orbit.Azimuth = 60;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => window.GameWindow.Title = $"{visual.Render(time.DeltaTime, orbit):F2}msec";
			window.Run();
			window.Dispose();
		}
	}
}