using OpenTK.Input;
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
			void updateMouseState(MouseEventArgs e)
			{
				var pos = window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y); //convert pixel coordinates to [0,1]²
				pos *= .5f;
				pos += System.Numerics.Vector2.One * .5f;
				var mouseState = new MouseState()
				{
					position = pos,
					drawState = GetDrawState(e.Mouse)
				};
				visual.MouseState = mouseState;
			}

			window.GameWindow.MouseMove += (s, a) => updateMouseState(a);
			window.GameWindow.MouseDown += (s, a) => updateMouseState(a);
			window.Render += visual.Render;
			window.Run();
		}

		private static int GetDrawState(OpenTK.Input.MouseState mouse)
		{
			if (mouse.IsButtonDown(MouseButton.Left))
			{
				return 1;
			}
			else if (mouse.IsButtonDown(MouseButton.Right))
			{
				return 2;
			}
			return 0;
		}
	}
}
