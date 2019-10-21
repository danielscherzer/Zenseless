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
			var camera = window.GameWindow.CreateOrbitingCameraController(30, 90, 0.1f, 500f);
//			var camera = window.GameWindow.CreateFirstPersonCameraController(1f, new Vector3(0, 0, 10), 70f, 0.1f, 20f);

			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += () => visual.Render(camera);
			window.Run();

		}
	}
}