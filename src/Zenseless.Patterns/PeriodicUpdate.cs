namespace Zenseless.Patterns
{
	/// <summary>
	/// Invokes a registered callback in regular intervals in the main thread 
	/// (important if you do for instance OpenGL stuff)
	/// </summary>
	/// <seealso cref="Zenseless.Patterns.ITimedUpdate" />
	public class PeriodicUpdate : ITimedUpdate
	{
		/// <summary>
		/// Gets how often the period has elapsed.
		/// </summary>
		/// <value>
		/// The period elapsed count.
		/// </value>
		public uint PeriodElapsedCount { get; private set; } = 0;
		
		/// <summary>
		/// Gets the period relative time. The time that has elapsed since the current period has started.
		/// </summary>
		/// <value>
		/// The time that has elapsed since the current period has started.
		/// </value>
		public float PeriodRelativeTime { get; private set; } = 0;

		/// <summary>
		/// Gets a value indicating whether this <see cref="PeriodicUpdate"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled - callback and internal time counting active from this point onward. 
		///   otherwise, <c>false - Stops invoking of the callback and internal time counting.</c>.
		/// </value>
		public bool Enabled { get; set; } = false;
		
		/// <summary>
		/// Event handler delegate type declaration
		/// </summary>
		/// <param name="sender">The <see cref="PeriodicUpdate"/> instance that invokes the callback.</param>
		/// <param name="absoluteTime">The absolute time at invoking.</param>
		public delegate void PeriodElapsedHandler(PeriodicUpdate sender, float absoluteTime);
		
		/// <summary>
		/// A registered callback is called each time the Interval period has elapsed.
		/// </summary>
		public event PeriodElapsedHandler PeriodElapsed;
		
		/// <summary>
		/// Gets or sets the period of time.
		/// </summary>
		/// <value>
		/// The period of time.
		/// </value>
		public float Period { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PeriodicUpdate"/> class.
		/// </summary>
		/// <param name="period">The regular time interval in which <see cref="PeriodElapsed"/> will be called.</param>
		public PeriodicUpdate(float period)
		{
			Period = period;
		}

		/// <summary>
		/// Updates the specified absolute time. 
		/// This method is responsible for calling the <see cref="PeriodElapsed"/> callback.
		/// This method has to be called at least once per frame to have frame exact callback evaluation.
		/// </summary>
		/// <param name="absoluteTime">The current absolute time.</param>
		public void Update(float absoluteTime)
		{
			if (!absoluteStartTime.HasValue) absoluteStartTime = absoluteTime;
			if (!Enabled)
			{
				absoluteStartTime = absoluteTime;
				PeriodRelativeTime = 0.0f;
				return;
			}
			PeriodRelativeTime = absoluteTime - absoluteStartTime.Value;
			if (PeriodRelativeTime > Period)
			{
				PeriodElapsed?.Invoke(this, absoluteTime);
				absoluteStartTime = absoluteTime;
				PeriodRelativeTime = 0.0f;
				++PeriodElapsedCount;
			}
		}

		/// <summary>
		/// The absolute start time in seconds
		/// </summary>
		private float? absoluteStartTime = null;
	}
}
