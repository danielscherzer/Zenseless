using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Texture" />
	/// <seealso cref="ITexture2dArray" />
	public class TextureArray2dGL : Texture, ITexture2dArray
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
		/// Gets the elements.
		/// </summary>
		/// <value>
		/// The elements.
		/// </value>
		public int Elements { get; private set; } = 1;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextureArray2dGL"/> class.
		/// </summary>
		public TextureArray2dGL(): base(TextureTarget.Texture2DArray) { }

		/// <summary>
		/// Loads the specified pixels.
		/// </summary>
		/// <param name="pixels">The pixels.</param>
		/// <param name="element">The element.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		public void Load(byte[] pixels, int element, byte components = 4, bool floatingPoint = false)
		{
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			Load(pixels, element, inputPixelFormat, type);
		}

		/// <summary>
		/// Loads the specified pixels.
		/// </summary>
		/// <param name="pixels">The pixels.</param>
		/// <param name="element">The element.</param>
		/// <param name="inputPixelFormat">The input pixel format.</param>
		/// <param name="type">The type.</param>
		public void Load(byte[] pixels, int element, PixelFormat inputPixelFormat, PixelType type)
		{
			Activate();
			GL.TexSubImage3D(Target, 0, 0, 0, element, Width, Height, 1, inputPixelFormat, type, pixels);
			Deactivate();
		}

		/// <summary>
		/// Sets the format.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="elements">The elements.</param>
		/// <param name="levels">The levels.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		public void SetFormat(int width, int height, int elements, int levels, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			SetFormat(width, height, elements, levels, (SizedInternalFormat)internalFormat);
		}

		/// <summary>
		/// Sets the format.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="elements">The elements.</param>
		/// <param name="levels">The levels.</param>
		/// <param name="internalFormat">The internal format.</param>
		public void SetFormat(int width, int height, int elements, int levels, SizedInternalFormat internalFormat)
		{
			Activate();
			GL.TexStorage3D(TextureTarget3d.Texture2DArray, levels, internalFormat, width, height, elements);
			this.Width = width;
			this.Height = height;
			this.Elements = elements;
			Deactivate();
		}
	}
}
