namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for the active render surface.
	/// </summary>
	public struct ActiveRenderSurface
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveRenderSurface" /> structure.
		/// </summary>
		/// <param name="renderSurface">The render surface.</param>
		public ActiveRenderSurface(IRenderSurface renderSurface)
		{
			RenderSurface = renderSurface;
		}

		/// <summary>
		/// Gets the shader program.
		/// </summary>
		/// <value>
		/// The shader program.
		/// </value>
		public IRenderSurface RenderSurface { get; }
	}
}
