namespace Example
{
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Application
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var orbit = window.GameWindow.CreateOrbitingCameraController(3, 70, 0.1f, 20f);
			orbit.Elevation = 15;

			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			window.Render += () => visual.Render(orbit);
			window.Run();
		}
	}
}