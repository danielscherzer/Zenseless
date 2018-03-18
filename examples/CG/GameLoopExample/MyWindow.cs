using OpenTK;
using System.Diagnostics;

namespace Example
{
	public class MyWindow : GameWindow
	{
		public MyWindow(int width = 512, int height = 512): base(width, height)
		{
			Visible = true; //show the window
			globalTime.Start();
		}

		public float GetTime()
		{
			return (float)globalTime.Elapsed.TotalSeconds;
		}

		public bool WaitForNextFrame()
		{
			SwapBuffers(); //double buffering
			ProcessEvents(); //handle all events that are sent to the window (user inputs, operating system stuff); this call could destroy window, so check immediately after this call if window still exists, otherwise GL calls will fail.
			return Exists;
		}

		private Stopwatch globalTime = new Stopwatch();
	}
}
