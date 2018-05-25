namespace Zenseless.Patterns
{
	using System;

	/// <summary>
	/// Class that implements exponential smoothing for series data
	/// https://en.wikipedia.org/wiki/Exponential_smoothing#Choosing_the_initial_smoothed_value
	/// </summary>
	public class ExponentialSmoothing
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExponentialSmoothing"/> class.
		/// </summary>
		/// <param name="stableWeight">The stable weight.</param>
		public ExponentialSmoothing(double stableWeight)
		{
			this.stableWeight = stableWeight;
			Clear();
		}

		/// <summary>
		/// Clears the series.
		/// </summary>
		public void Clear()
		{
			SmoothedValue = 0f;
			weight = 1.0; //initially only use new sample value
		}

		/// <summary>
		/// Gets the current smoothed value.
		/// </summary>
		/// <value>
		/// The smoothed value.
		/// </value>
		public double SmoothedValue { get; private set; }

		/// <summary>
		/// Adds a new sample to the series.
		/// </summary>
		/// <param name="value">The value.</param>
		public void NewSample(double value)
		{
			SmoothedValue = weight * value + (1.0 - weight) * SmoothedValue;
			weight = Math.Max(stableWeight, weight - stableWeight); //needed to avoid initial drift
		}

		private readonly double stableWeight;
		private double weight;
	}
}
