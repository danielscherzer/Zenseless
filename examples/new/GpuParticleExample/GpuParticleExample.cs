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
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.AddMayaCameraEvents(visual.OrbitCamera);
			var time = new GameTime();
			window.Render += () => window.GameWindow.Title = $"{visual.Render(time.DeltaTime):F2}msec";
			window.Run();
		}
	}
}