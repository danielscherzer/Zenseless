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
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.AddMayaCameraEvents(visual.OrbitCamera);

			var time = new GameTime();
			window.Render += visual.Render;
			window.Update += (t) => visual.Update(time.AbsoluteTime);
			window.Run();
		}
	}
}