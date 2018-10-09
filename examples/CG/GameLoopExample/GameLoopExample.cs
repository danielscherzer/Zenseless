using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Example
{
	public class Program
	{
		[STAThread]
		private static void Main()
		{
			var wnd = new MyWindow();
			wnd.KeyDown += (s, a) => { if (Key.Escape == a.Key) wnd.Close(); }; //if Escape is pressed close window
																				//wnd.VSync = VSyncMode.Off; //uncomment for pc speed dependent rendering
			
			float count = 1;
			var x = -1f;
			GL.ClearColor(Color.CornflowerBlue);
			var rnd = new Random(12);

			//main loop
			while (wnd.WaitForNextFrame())
			{
				if (x < 1) x +=  0.01f; else x = -1; //move to right animation
				GL.Clear(ClearBufferMask.ColorBufferBit);
				for (int i = 0; i < count; ++i)
				{
					var y = (float)rnd.NextDouble() - 0.5f;
					//draw a primitive
					GL.Begin(PrimitiveType.LineLoop);
					//color is active as long as no new color is set
					GL.Color3(Color.White);
					GL.Vertex2(x + 0f, y + 0f);
					GL.Vertex2(x + .5f, y + 0f);
					GL.Vertex2(x + .5f, y + .5f);
					GL.Vertex2(x + .0f, y + .5f);
					GL.End();
				}
				count *= 1.02f;
			}
		}
	}
}
