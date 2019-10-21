namespace Heightfield
{
	using System.Numerics;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Heightfield
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			//var camera = window.GameWindow.CreateOrbitingCameraController(1f, 70f, 0.01f, 20f);
			//camera.View.Elevation = 30;

			var camera = window.GameWindow.CreateFirstPersonCameraController(0.1f, new Vector3(0.4f, 0.4f, 0.95f), 70f, 0.01f, 30f);

			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			window.Render += () => visual.Render(camera);
			window.Run();

		}
	}
}
