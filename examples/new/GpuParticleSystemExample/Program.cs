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
			var orbit = window.GameWindow.CreateOrbitingCameraController(2, 70, 0.1f, 20f);
			orbit.Elevation = 15;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => visual.Render(time.DeltaTime, orbit);
			window.Run();
			window.Dispose();
		}
	}
}