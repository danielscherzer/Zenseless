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
			var orbit = window.GameWindow.CreateOrbitingCameraController(3, 70, 0.1f, 20);
			orbit.Elevation = 35;
			orbit.Azimuth = 60;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => window.GameWindow.Title = $"{visual.Render(time.DeltaTime, orbit):F2}msec";
			window.Run();
		}
	}
}