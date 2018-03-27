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
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var orbit = window.GameWindow.CreateOrbitingCameraController(1.8f, 70, 0.1f, 50f);
			orbit.TargetY = -0.3f;

			window.Render += () => visual.Render(orbit, orbit.CalcPosition());
			window.Run();
		}
	}
}