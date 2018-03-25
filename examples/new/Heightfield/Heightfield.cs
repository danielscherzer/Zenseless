using System.IO;
using Zenseless.Base;
using Zenseless.ExampleFramework;
using Zenseless.OpenGL;

namespace Heightfield
{
	class Heightfield
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath()));
			var visual = new MainVisual(window.RenderContext, window.ContentLoader);
			var movementState = window.GameWindow.AddFirstPersonCameraEvents(visual.Camera);
			window.Update += (dt) => movementState.Update(visual.Camera, dt);
			window.Render += visual.Render;
			window.Run();
		}
	}
}
