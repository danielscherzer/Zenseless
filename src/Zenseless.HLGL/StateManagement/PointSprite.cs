namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for point sprite generation.
	/// </summary>
	public struct PointSprite
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PointSprite"/> structure.
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public PointSprite(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="PointSprite"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; }
	}
}
