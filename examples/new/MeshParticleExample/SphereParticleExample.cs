using System;
using System.IO;
using Zenseless.Base;
using Zenseless.ExampleFramework;
using Zenseless.OpenGL;

namespace Example
{
	class Application
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath())); //would be faster if you only specify a subdirectory
			var orbit = window.GameWindow.CreateOrbitingCameraController(3, 70, 0.1f, 20f);
			orbit.Elevation = 15;

			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			window.Render += () => visual.Render(orbit);
			window.Run();
		}
	}
}