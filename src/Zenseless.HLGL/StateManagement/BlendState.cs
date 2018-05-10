namespace Zenseless.HLGL
{
	using System;

	/// <summary>
	/// Encapsulates the blend state inside an immutable structure
	/// </summary>
	public struct BlendState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlendState"/> structure.
		/// </summary>
		/// <param name="blendOperator">The blend function operator.</param>
		/// <param name="blendParameterSource">The source blend parameter.</param>
		/// <param name="blendParameterDestination">The destination blend parameter.</param>
		public BlendState(BlendOperator blendOperator, BlendParameter blendParameterSource, BlendParameter blendParameterDestination)
		{
			BlendOperator = blendOperator;
			BlendParameterSource = blendParameterSource;
			BlendParameterDestination = blendParameterDestination;
		}

		/// <summary>
		/// Gets the blend function operator.
		/// </summary>
		/// <value>
		/// The blend function operator.
		/// </value>
		public BlendOperator BlendOperator { get; }
		/// <summary>
		/// Gets the blend parameter source.
		/// </summary>
		/// <value>
		/// The blend parameter source.
		/// </value>
		public BlendParameter BlendParameterSource { get; }
		/// <summary>
		/// Gets the blend parameter destination.
		/// </summary>
		/// <value>
		/// The blend parameter destination.
		/// </value>
		public BlendParameter BlendParameterDestination { get; }
	}
}
