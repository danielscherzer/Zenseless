namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IRenderSurface
	{
		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		ITexture Texture { get; }
		/// <summary>
		/// Clears this instance.
		/// </summary>
		void Clear();
		/// <summary>
		/// Draws the specified configuration.
		/// </summary>
		/// <param name="config">The configuration.</param>
		void Draw(IDrawConfiguration config);
	}
}