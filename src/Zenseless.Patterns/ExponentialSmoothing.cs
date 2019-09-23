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
		/// <param name="weight">The weight of a new sample. between ]0,1[.</param>
		public ExponentialSmoothing(double weight)
		{
			if ((0 >= weight) || (1 <= weight)) throw new ArgumentException("Stable weight has to be inside ]0,1[");
			stableWeight = weight;
			Clear();
		}

		/// <summary>
		/// Clears the series.
		/// </summary>
		public void Clear()
		{
			SmoothedValue = 0f;
			currentWeight = 1.0; //initially only use new sample value
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
			SmoothedValue = currentWeight * value + (1.0 - currentWeight) * SmoothedValue;
			currentWeight = Math.Max(stableWeight, currentWeight - stableWeight); //needed to avoid initial drift
		}

		private readonly double stableWeight;
		private double currentWeight;
	}
}
