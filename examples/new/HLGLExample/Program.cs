using System;
using System.IO;
using Zenseless.Base;
using Zenseless.ExampleFramework;
using Zenseless.OpenGL;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath())); //would be faster if you only specify a subdirectory
			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			window.GameWindow.AddMayaCameraEvents(visual.Camera);
			window.Render += visual.Render;
			window.Resize += visual.Resize;
			window.Run();
		}
	}
}
