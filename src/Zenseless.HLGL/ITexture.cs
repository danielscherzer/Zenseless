using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Enumeration of texture filtering modes
	/// </summary>
	public enum TextureFilterMode
	{
		/// <summary>
		/// Nearest neighbor filter mode (box filtering)
		/// </summary>
		Nearest,
		/// <summary>
		/// Linear filter mode (tent filtering)
		/// </summary>
		Linear,
		/// <summary>
		/// Mipmap filter mode
		/// </summary>
		Mipmap
	};
	/// <summary>
	/// Enumeration of texture wrap function
	/// </summary>
	public enum TextureWrapFunction
	{
		/// <summary>
		/// The repeat
		/// </summary>
		Repeat,
		/// <summary>
		/// The mirrored repeat
		/// </summary>
		MirroredRepeat,
		/// <summary>
		/// The clamp to edge
		/// </summary>
		ClampToEdge,
		/// <summary>
		/// The clamp to border
		/// </summary>
		ClampToBorder
	};

	/// <summary>
	/// Base texture interface
	/// </summary>
	/// <seealso cref="IDisposable" />
	public interface ITexture : IDisposable
	{
		/// <summary>
		/// Gets or sets the filter.
		/// </summary>
		/// <value>
		/// The filter.
		/// </value>
		TextureFilterMode Filter { get; set; }
		
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		uint ID { get; }
		
		/// <summary>
		/// Gets or sets the wrap function.
		/// </summary>
		/// <value>
		/// The wrap function.
		/// </value>
		TextureWrapFunction WrapFunction { get; set; }

		/// <summary>
		/// Activates this instance.
		/// </summary>
		void Activate();
		
		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		void Deactivate();
	}
}