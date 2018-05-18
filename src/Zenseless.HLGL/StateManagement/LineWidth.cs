namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public struct LineWidth : IState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LineWidth"/> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public LineWidth(float value)
		{
			Value = value;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public float Value { get; }
	}
}
