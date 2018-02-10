namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.IO;
	using Zenseless.HLGL;
	using SysMedia = System.Windows.Media;

	/// <summary>
	/// 
	/// </summary>
	public static class TextureLoaderImaging
	{
		/// <summary>
		/// Froms the stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		public static ITexture FromStream(Stream stream)
		{
			var texture = new Texture2dGL
			{
				Filter = TextureFilterMode.Mipmap
			};
			texture.Activate();
			var source = new SysMedia.Imaging.BitmapImage();
			source.BeginInit();
			source.StreamSource = stream;
			source.EndInit();
			//TODO: flip bitmap image
			var writable = new SysMedia.Imaging.WriteableBitmap(source);
			writable.Lock();
			var internalFormat = SelectInternalPixelFormat(source.Format);
			var inputPixelFormat = SelectPixelFormat(source.Format);
			texture.LoadPixels(writable.BackBuffer, source.PixelWidth, source.PixelHeight, internalFormat, inputPixelFormat, PixelType.UnsignedByte);
			writable.Unlock();
			texture.Deactivate();
			return texture;
		}

		/// <summary>
		/// Selects the internal pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Wrong pixel format " + pixelFormat.ToString()</exception>
		private static PixelInternalFormat SelectInternalPixelFormat(SysMedia.PixelFormat pixelFormat)
		{
			if (SysMedia.PixelFormats.Bgra32 == pixelFormat)
			{
				return PixelInternalFormat.Rgba;
			}
			else if (SysMedia.PixelFormats.Rgb24 == pixelFormat)
			{
				return PixelInternalFormat.Rgb;
			}
			else if (SysMedia.PixelFormats.Gray8 == pixelFormat)
			{
				return PixelInternalFormat.R8;
			}
			else throw new ArgumentException("Wrong pixel format " + pixelFormat.ToString());
		}

		/// <summary>
		/// Selects the pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Wrong pixel format " + pixelFormat.ToString()</exception>
		private static PixelFormat SelectPixelFormat(SysMedia.PixelFormat pixelFormat)
		{
			if (SysMedia.PixelFormats.Bgra32 == pixelFormat)
			{
				return PixelFormat.Bgra;
			}
			else if (SysMedia.PixelFormats.Rgb24 == pixelFormat)
			{
				return PixelFormat.Bgr;
			}
			else if (SysMedia.PixelFormats.Gray8 == pixelFormat)
			{
				return PixelFormat.Red;
			}
			else throw new ArgumentException("Wrong pixel format " + pixelFormat.ToString());
		}
	}
}
