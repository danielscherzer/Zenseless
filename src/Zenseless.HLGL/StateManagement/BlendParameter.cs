namespace Zenseless.HLGL
{
	/// <summary>
	/// Represents the available blending parameters.
	/// </summary>
	public enum BlendParameter
	{
		/// <summary>
		/// The components of the blended color are multiplied by zero.
		/// </summary>
		Zero,

		/// <summary>
		/// The components of the blended color are multiplied by one.
		/// </summary>
		One,

		/// <summary>
		/// The components of the blended color are multiplied by the source color.
		/// </summary>
		SourceColor,

		/// <summary>
		/// The components of the blended color are multiplied by (1, 1, 1, 1) - the source color.
		/// </summary>
		OneMinusSourceColor,

		/// <summary>
		/// The components of the blended color are multiplied by the source alpha.
		/// </summary>
		SourceAlpha,

		/// <summary>
		/// The components of the blended color are multiplied by 1 - the source alpha.
		/// </summary>
		OneMinusSourceAlpha,

		/// <summary>
		/// The components of the blended color are multiplied by the destination alpha.
		/// </summary>
		DestinationAlpha,

		/// <summary>
		/// The components of the blended color are multiplied by 1 - the destination alpha.
		/// </summary>
		OneMinusDestinationAlpha,

		/// <summary>
		/// The components of the blended color are multiplied by the destination color.
		/// </summary>
		DestinationColor,

		/// <summary>
		/// The components of the blended color are multiplied by (1, 1, 1, 1) - the destination color.
		/// </summary>
		OneMinusDestinationColor,

		/// <summary>
		/// The components of the blended color are multiplied by the larger of 
		/// the alpha of the source color or 1 - the alpha of the source color.
		/// </summary>
		SourceAlphaSaturation,
	}
}