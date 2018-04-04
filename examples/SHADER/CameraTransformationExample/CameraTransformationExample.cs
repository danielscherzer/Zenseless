using OpenTK.Input;
using System;
using System.IO;
using Zenseless.Base;
using Zenseless.ExampleFramework;

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
			window.GameWindow.MouseMove += (s, e) =>
			{
				if (ButtonState.Pressed == e.Mouse.LeftButton)
				{
					visual.CameraAzimuth += 300 * e.XDelta / (float)window.GameWindow.Width;
					visual.CameraElevation += 300 * e.YDelta / (float)window.GameWindow.Height;
				}
			};
			window.GameWindow.MouseWheel += (s, e) => visual.CameraDistance *= (float)Math.Pow(1.05, e.DeltaPrecise);
			window.Update += visual.Update;
			window.Render += visual.Render;
			window.Run();
		}
	}
}