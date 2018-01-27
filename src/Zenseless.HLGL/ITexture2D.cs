using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="ITexture" />
	public interface ITexture2D : ITexture
	{
		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>
		/// The height.
		/// </value>
		int Height { get; }
		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		int Width { get; }

		/// <summary>
		/// Loads the pixels.
		/// </summary>
		/// <param name="pixels">The pixels.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		void LoadPixels(IntPtr pixels, int width, int height, byte components = 4, bool floatingPoint = false);
	}
}