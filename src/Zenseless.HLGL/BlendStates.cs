namespace Zenseless.HLGL
{
	/// <summary>
	/// Predefined commonly used blend states
	/// </summary>
	public static class BlendStates
	{
		/// <summary>
		/// Additive blending
		/// </summary>
		public static BlendState Additive = new BlendState(BlendOperator.Add, BlendParameter.SourceAlpha, BlendParameter.One);
		/// <summary>
		/// Alpha blending (transparency)
		/// </summary>
		public static BlendState AlphaBlend = new BlendState(BlendOperator.Add, BlendParameter.SourceAlpha, BlendParameter.OneMinusSourceAlpha);
		/// <summary>
		/// Opaque blending (blending disabled)
		/// </summary>
		public static BlendState Opaque = new BlendState(BlendOperator.None, BlendParameter.One, BlendParameter.Zero);
	}
}
