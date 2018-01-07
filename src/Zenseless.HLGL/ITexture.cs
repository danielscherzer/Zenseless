using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public enum TextureFilterMode
	{
		/// <summary>
		/// The nearest
		/// </summary>
		Nearest,
		/// <summary>
		/// The linear
		/// </summary>
		Linear,
		/// <summary>
		/// The mipmap
		/// </summary>
		Mipmap
	};
	/// <summary>
	/// 
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
	/// 
	/// </summary>
	/// <seealso cref="System.IDisposable" />
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