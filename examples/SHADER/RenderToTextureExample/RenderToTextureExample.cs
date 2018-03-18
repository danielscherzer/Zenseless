using OpenTK.Input;
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

			var globalTime = new GameTime();
			bool doPostProcessing = false;

			window.Render += () =>
			{
				if (doPostProcessing)
				{
					visual.DrawWithPostProcessing(globalTime.AbsoluteTime);
				}
				else
				{
					visual.Draw();
				}
			};

			window.Update += (t) => doPostProcessing = !Keyboard.GetState()[Key.Space];
			window.Resize += visual.Resize;
			window.GameWindow.AddMayaCameraEvents(visual.OrbitCamera);
			window.Run();
		}
	}
}