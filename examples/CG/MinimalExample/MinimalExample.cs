using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Example
{
	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new GameWindow(512, 512);

			void draw()
			{
				//clear screen - what happens without?
				GL.Clear(ClearBufferMask.ColorBufferBit);
				//draw a quad
				GL.Begin(PrimitiveType.Quads);
				GL.Vertex2(0.0f, 0.0f); //draw first quad corner
				GL.Vertex2(0.5f, 0.0f);
				GL.Vertex2(0.5f, 0.5f);
				GL.Vertex2(0.0f, 0.5f);
				GL.End();
				window.SwapBuffers(); // get a frame-buffer to draw to (double/tripple buffering)
			}

			window.RenderFrame += (s, a) => draw();
			window.Run();

		}
	}
}