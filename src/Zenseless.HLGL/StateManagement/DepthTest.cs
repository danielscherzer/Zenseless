namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for depth test.
	/// </summary>
	public struct DepthTest : IState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DepthTest"/> structure.
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public DepthTest(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="DepthTest"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; }
	}
}
