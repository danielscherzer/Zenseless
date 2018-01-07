using System.Diagnostics;

namespace Zenseless.Base
{
	/// <summary>
	/// Intended as a source for time.
	/// Can do frames-per-second counting.
	/// Uses a <see cref="System.Diagnostics.Stopwatch" />
	/// </summary>
	public class GameTime : ITime
	{
		/// <summary>
		/// Gets the time since the last frame.
		/// </summary>
		/// <value>
		/// The delta time in seconds.
		/// </value>
		public float DeltaTime { get; private set; }
		/// <summary>
		/// Gets the current frames-per-second.
		/// </summary>
		/// <value>
		/// Frames-per-second as a float.
		/// </value>
		public float FPS { get; private set; }

		/// <summary>
		/// Gets the absolute time since start in seconds.
		/// </summary>
		/// <value>
		/// The absolute time in seconds.
		/// </value>
		public float AbsoluteTime => (float)stopwatch.Elapsed.TotalSeconds;
		/// <summary>
		/// Gets the elapsed time in milliseconds.
		/// </summary>
		/// <value>
		/// The time in milliseconds.
		/// </value>
		public float AbsoluteMilliseconds => (float)stopwatch.Elapsed.TotalMilliseconds;

		/// <summary>
		/// Initializes a new instance of the <see cref="GameTime"/> class.
		/// This will start the time counting
		/// </summary>
		public GameTime()
		{
			DeltaTime = 1f / 60f;
			FPS = 60;
			stopwatch.Start();
		}

		/// <summary>
		/// Start a new frame. You have to call this method exactly once per frame for correct FPS counting and delta time.
		/// </summary>
		public void NewFrame()
		{
			var time = AbsoluteTime;
			DeltaTime = time - lastRenderTime;
			lastRenderTime = time;

			++frames;
			long newTime = stopwatch.ElapsedMilliseconds;
			long diff = newTime - lastFpsUpdateTime;
			if (diff > 500)
			{
				FPS = (1000f * frames) / diff;
				lastFpsUpdateTime = newTime;
				frames = 0;
			}
		}

		private uint frames = 0;
		private long lastFpsUpdateTime = 0;
		private float lastRenderTime = 0f;
		private Stopwatch stopwatch = new Stopwatch();
	}
}