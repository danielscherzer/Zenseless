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
			var orbit = window.GameWindow.CreateOrbitingCameraController(0, 70, 0.01f, 50f);
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += () => visual.Render(orbit, orbit.CalcPosition());
			window.Run();

		}
	}
}