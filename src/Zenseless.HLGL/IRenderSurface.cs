namespace Zenseless.HLGL
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// 
	/// </summary>
	public interface IRenderSurface : IDisposable
	{
		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		ITexture2D Texture { get; }
		/// <summary>
		/// Gets the textures.
		/// </summary>
		/// <value>
		/// The textures.
		/// </value>
		IReadOnlyList<ITexture2D> Textures { get; }

		/// <summary>
		/// Activates this instance.
		/// </summary>
		void Activate();

		/// <summary>
		/// Draws onto the render surface using the specified draw code.
		/// </summary>
		/// <param name="drawCode">The draw code.</param>
		void Draw(Action drawCode);

		/// <summary>
		/// Attaches the specified texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		void Attach(ITexture2D texture);

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		void Deactivate();
	}
}