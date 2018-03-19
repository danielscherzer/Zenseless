using System.IO;
using Zenseless.Base;
using Zenseless.ExampleFramework;

namespace Heightfield
{
	class Heightfield
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath())); //would be faster if you only specify a subdirectory
			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			//window.GameWindow.AddMayaCameraEvents(visual.Camera);
			window.Render += visual.Render;
			window.Run();
		}
	}
}
