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

			var camera = window.GameWindow.CreateOrbitingCameraController(1.8f, 70, 0.1f, 50f);
			camera.TargetY = -0.3f;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var globalTime = new GameTime();
			bool doPostProcessing = false;

			window.Render += () =>
			{
				if (doPostProcessing)
				{
					visual.DrawWithPostProcessing(globalTime.AbsoluteTime, camera);
				}
				else
				{
					visual.Draw(camera);
				}
			};

			window.Update += (t) => doPostProcessing = !Keyboard.GetState()[Key.Space];
			window.Resize += visual.Resize;
			window.Run();
		}
	}
}