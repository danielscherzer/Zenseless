namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Drawing;
	using System.IO;
	using Zenseless.HLGL;
	using SysDraw = System.Drawing.Imaging;

	/// <summary>
	/// 
	/// </summary>
	public static class TextureLoaderDrawing
	{
		/// <summary>
		/// Load a texture from a bitmap.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		public static ITexture2D FromBitmap(Bitmap bitmap)
		{
			var texture = new Texture2dGL();
			texture.FromBitmap(bitmap);
			return texture;
		}

		/// <summary>
		/// Loads texture date from a bitmap into a Texture2dGL instance.
		/// </summary>
		/// <param name="texture">The texture instance.</param>
		/// <param name="bitmap">The bitmap.</param>
		public static void FromBitmap(this Texture2dGL texture, Bitmap bitmap)
		{
			//TODO: 16bit channels
			using (Bitmap bmp = new Bitmap(bitmap))
			{
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), SysDraw.ImageLockMode.ReadOnly, bmp.PixelFormat);
				var internalFormat = SelectInternalPixelFormat(bmp.PixelFormat);
				var inputPixelFormat = SelectPixelFormat(bmp.PixelFormat);
				texture.LoadPixels(bmpData.Scan0, bmpData.Width, bmpData.Height, internalFormat, inputPixelFormat, PixelType.UnsignedByte);
				bmp.UnlockBits(bmpData);
			}
			texture.Filter = TextureFilterMode.Mipmap;
		}

		/// <summary>
		/// Load a texture from a file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="FileLoadException"></exception>
		public static ITexture2D FromFile(string fileName)
		{
			if (String.IsNullOrEmpty(fileName))
			{
				throw new ArgumentException(fileName);
			}
			if (!File.Exists(fileName))
			{
				throw new FileLoadException(fileName);
			}
			return FromBitmap(new Bitmap(fileName));
		}

		/// <summary>
		/// Saves to file.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="format">The format.</param>
		public static void SaveToFile(this ITexture2D texture, string fileName, SysDraw.PixelFormat format = SysDraw.PixelFormat.Format32bppArgb)
		{
			using (var bitmap = SaveToBitmap(texture, format))
			{
				bitmap.Save(fileName);
			}
		}

		/// <summary>
		/// Saves to bitmap.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public static Bitmap SaveToBitmap(this ITexture2D texture, SysDraw.PixelFormat format = SysDraw.PixelFormat.Format32bppArgb)
		{
			try
			{ 
				var bmp = new Bitmap(texture.Width, texture.Height);
				texture.Activate();
				var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), SysDraw.ImageLockMode.WriteOnly, format);
				GL.GetTexImage(TextureTarget.Texture2D, 0, SelectPixelFormat(format), PixelType.UnsignedByte, data.Scan0);
				bmp.UnlockBits(data);
				texture.Deactivate();
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				return bmp;
			}
			catch
			{
				texture.Deactivate();
				return null;
			}
		}

		/// <summary>
		/// Selects the pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Wrong pixel format " + pixelFormat.ToString()</exception>
		public static PixelFormat SelectPixelFormat(SysDraw.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case SysDraw.PixelFormat.Format8bppIndexed: return PixelFormat.Red;
				case SysDraw.PixelFormat.Format24bppRgb: return PixelFormat.Bgr;
				case SysDraw.PixelFormat.Format32bppArgb: return PixelFormat.Bgra;
				default: throw new ArgumentException("Wrong pixel format " + pixelFormat.ToString());
			}
		}

		/// <summary>
		/// Selects the internal pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Wrong pixel format " + pixelFormat.ToString()</exception>
		public static PixelInternalFormat SelectInternalPixelFormat(SysDraw.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case SysDraw.PixelFormat.Format8bppIndexed: return PixelInternalFormat.R8;
				case SysDraw.PixelFormat.Format24bppRgb: return PixelInternalFormat.Rgb8;
				case SysDraw.PixelFormat.Format32bppArgb: return PixelInternalFormat.Rgba8;
				default: throw new ArgumentException("Wrong pixel format " + pixelFormat.ToString());
			}
		}
	}
}
