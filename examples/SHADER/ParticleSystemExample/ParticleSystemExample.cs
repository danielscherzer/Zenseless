using System;
using System.IO;
using Zenseless.Base;
using Zenseless.ExampleFramework;
using Zenseless.OpenGL;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath())); //would be faster if you only specify a subdirectory
			var controller = new Controller();

			var camera = window.GameWindow.CreateOrbitingCameraController(2, 70, 0.1f, 20f);
			camera.Elevation = 15;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var time = new GameTime();
			window.Render += () => visual.Render(camera);
			window.Update += (t) => visual.Update(time.AbsoluteTime);
			window.Run();
		}
	}
}