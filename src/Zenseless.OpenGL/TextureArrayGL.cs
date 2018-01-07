using OpenTK.Graphics.OpenGL4;
using System;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.OpenGL.Texture" />
	public class TextureArrayGL : Texture
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
		/// Initializes a new instance of the <see cref="TextureArrayGL"/> class.
		/// </summary>
		public TextureArrayGL(): base(TextureTarget.Texture2DArray) { }

		/// <summary>
		/// Loads the specified pixels.
		/// </summary>
		/// <param name="pixels">The pixels.</param>
		/// <param name="element">The element.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		public void Load(IntPtr pixels, int element, byte components = 4, bool floatingPoint = false)
		{
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			Activate();
			GL.TexSubImage3D(Target, 0, 0, 0, 0, Width, Height, element, inputPixelFormat, type, pixels);
			Deactivate();
		}

		/// <summary>
		/// Sets the format.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="elements">The elements.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		public void SetFormat(int width, int height, int elements, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			Activate();
			GL.TexStorage3D(TextureTarget3d.Texture2DArray, 0, SizedInternalFormat.R16, width, height, elements);
			this.Width = width;
			this.Height = height;
			this.Elements = elements;
			Deactivate();
		}
	}
}
