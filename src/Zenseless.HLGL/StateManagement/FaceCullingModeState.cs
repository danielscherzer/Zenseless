namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for face culling. Mostly used for enabling back-face culling
	/// </summary>
	public struct FaceCullingModeState : IState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FaceCullingModeState"/> structure.
		/// </summary>
		/// <param name="faceCullingMode">The face culling mode.</param>
		public FaceCullingModeState(FaceCullingMode faceCullingMode)
		{
			Mode = faceCullingMode;
		}

		/// <summary>
		/// Gets the face culling mode.
		/// </summary>
		/// <value>
		/// The face culling mode.
		/// </value>
		public FaceCullingMode Mode { get; }
	}
}
