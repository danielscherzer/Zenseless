namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for anti-aliasing of lines (requires blending to work).
	/// </summary>
	public struct LineSmoothing : IState
	{
		/// <summary>
		/// Anti-aliasing of lines (requires blending to work)
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [value].</param>
		public LineSmoothing(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>
		/// Gets a value indicating whether <see cref="LineSmoothing"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; }
	}
}
