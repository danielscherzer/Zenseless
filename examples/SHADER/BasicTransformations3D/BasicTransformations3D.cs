using Zenseless.ExampleFramework;
using Zenseless.Base;
using System;
using System.IO;

namespace Example
{
	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath())); //would be faster if you only specify a subdirectory
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var time = new GameTime();
			window.Render += visual.Render;
			window.Update += (dt) => visual.Update(time.AbsoluteTime);
			//window.IsRecording = true;
			window.Run();
		}
	}
}
