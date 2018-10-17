using Zenseless.ExampleFramework;
using Zenseless.OpenGL;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow(debug:true);
			var view = new View(window.ContentLoader, window.RenderContext);
			var camera = window.GameWindow.CreateOrbitingCameraController(10f, 70f, 1f, 2000f);
			window.Render += () => view.Draw(camera);
			window.Run();

		}
	}
}
