using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using Zenseless.Patterns;

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
			GL.Finish();
			var timeBefore = globalTime.ElapsedMilliseconds;
			SwapBuffers(); //double buffering
			var timeAfter = globalTime.ElapsedMilliseconds;
			timeSeries.NewSample(timeAfter - timeBefore);
			Title = $"Time spent in swap buffer time ={timeSeries.SmoothedValue:F2}ms";
			ProcessEvents(); //handle all events that are sent to the window (user inputs, operating system stuff); this call could destroy window, so check immediately after this call if window still exists, otherwise GL calls will fail.
			return Exists;
		}

		private Stopwatch globalTime = new Stopwatch();
		private ExponentialSmoothing timeSeries = new ExponentialSmoothing(0.01);
	}
}
