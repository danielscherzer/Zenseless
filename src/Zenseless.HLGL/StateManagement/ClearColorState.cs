namespace Zenseless.HLGL
{
	/// <summary>
	/// The color used by the graphic system for clearing the screen.
	/// </summary>
	public struct ClearColorState : IState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClearColorState"/> struct.
		/// </summary>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		/// <param name="alpha">The alpha.</param>
		public ClearColorState(float red, float green, float blue, float alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		/// <summary>
		/// Gets the red.
		/// </summary>
		/// <value>
		/// The red.
		/// </value>
		public float Red { get; }
		/// <summary>
		/// Gets the green.
		/// </summary>
		/// <value>
		/// The green.
		/// </value>
		public float Green { get; }
		/// <summary>
		/// Gets the blue.
		/// </summary>
		/// <value>
		/// The blue.
		/// </value>
		public float Blue { get; }
		/// <summary>
		/// Gets the alpha.
		/// </summary>
		/// <value>
		/// The alpha.
		/// </value>
		public float Alpha { get; }
	}
}
