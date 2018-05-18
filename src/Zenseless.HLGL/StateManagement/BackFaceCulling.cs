namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for back face culling.
	/// </summary>
	public struct BackFaceCulling : IState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BackFaceCulling"/> structure.
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public BackFaceCulling(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="BackFaceCulling"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; }
	}
}
