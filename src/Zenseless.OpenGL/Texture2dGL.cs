﻿using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Texture" />
	/// <seealso cref="ITexture2D" />
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
			//if(width != Width ||height != Height)
			//GL.TexStorage2D((TextureTarget2d)Target, 1, (SizedInternalFormat)internalFormat, width, height); //immutable texture storage need to set mipmap levels
			Width = width;
			Height = height;
			//if (IntPtr.Zero != pixels) GL.TexSubImage2D(Target, 0, 0, 0, width, height, inputPixelFormat, type, pixels);
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);

			Deactivate();
		}

		/// <summary>
		/// Loads the pixels.
		/// </summary>
		/// <param name="pixels">The 2-dim pixel array.</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		public void LoadPixels<T>(T[,] pixels, byte components = 4, bool floatingPoint = false) where T : struct
		{
			var width = pixels.GetLength(0);
			var height = pixels.GetLength(1);
			var internalFormat = Convert(components, false);
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.Float : PixelType.UnsignedByte;
			Activate();
			//if (width != Width || height != Height)
			//GL.TexStorage2D((TextureTarget2d)Target, 1, (SizedInternalFormat)internalFormat, width, height); //immutable texture storage need to set mipmap levels
			Width = width;
			Height = height;
			//if (IntPtr.Zero != pixels) GL.TexSubImage2D(Target, 0, 0, 0, width, height, inputPixelFormat, type, pixels);
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);

			Deactivate();
		}
	}
}
