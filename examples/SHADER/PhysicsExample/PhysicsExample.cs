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
			var model = new Model();
			var camera = window.GameWindow.CreateOrbitingCameraController(30, 90, 0.1f, 500f);
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var time = new GameTime();
			window.Render += () => visual.Render(model.Bodies, time.AbsoluteTime, camera);
			window.Update += model.Update;
			window.Run();
		}
	}
}