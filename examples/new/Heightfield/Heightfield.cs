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
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath()));
			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			window.AddMayaCameraEvents(visual.Camera);
			window.Render += visual.Render;
			window.Run();
		}
	}
}
