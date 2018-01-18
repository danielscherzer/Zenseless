using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.OpenGL.Texture" />
	/// <seealso cref="Zenseless.HLGL.ITexture2D" />
	public class Texture2dGL : Texture, ITexture2D
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		public int Width { get; private set; } = 0;
		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>
		/// The height.
		/// </value>
		public int Height { get; private set; } = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="Texture2dGL"/> class.
		/// </summary>
		public Texture2dGL(): base(TextureTarget.Texture2D) { }

		/// <summary>
		/// Creates the specified width.
		/// </summary>
		/// <param name="width">The texture width in pixel.</param>
		/// <param name="height">The texture height in pixel.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		/// <returns></returns>
		public static Texture2dGL Create(int width, int height, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			return Create(width, height, internalFormat, inputPixelFormat, type);
		}

		/// <summary>
		/// Creates the specified width.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="internalFormat">The internal format.</param>
		/// <param name="inputPixelFormat">The input pixel format.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static Texture2dGL Create(int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat = PixelFormat.Rgba, PixelType type = PixelType.UnsignedByte)
		{
			var texture = new Texture2dGL();
			//create empty texture of given size
			texture.LoadPixels(IntPtr.Zero, width, height, internalFormat, inputPixelFormat, type);
			//set default parameters for filtering and clamping
			texture.Filter = TextureFilterMode.Linear;
			texture.WrapFunction = TextureWrapFunction.Repeat;
			return texture;
		}

		/// <summary>
		/// Loads the pixels.
		/// </summary>
		/// <param name="pixels">The pixels.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="internalFormat">The internal format.</param>
		/// <param name="inputPixelFormat">The input pixel format.</param>
		/// <param name="type">The type.</param>
		public void LoadPixels(IntPtr pixels, int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat, PixelType type)
		{
			Activate();
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			Deactivate();
		}

		/// <summary>
		/// Loads the pixels.
		/// </summary>
		/// <param name="pixels">The pixels.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		public void LoadPixels(IntPtr pixels, int width, int height, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			Activate();
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			Deactivate();
		}
	}
}
