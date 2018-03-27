using Zenseless.ExampleFramework;
using Zenseless.Base;
using System;
using System.IO;
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
			var orbit = window.GameWindow.CreateOrbitingCameraController(30, 90, 0.1f, 500f);
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += () => visual.Render(orbit);
			window.Run();
		}
	}
}