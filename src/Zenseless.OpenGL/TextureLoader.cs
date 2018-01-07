using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Numerics;

namespace Zenseless.OpenGL
{
	using SysDraw = System.Drawing.Imaging;
	using SysMedia = System.Windows.Media;

	/// <summary>
	/// 
	/// </summary>
	public static class TextureLoader
	{
		/// <summary>
		/// Froms the array.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <param name="data">The data.</param>
		/// <param name="internalFormat">The internal format.</param>
		/// <param name="format">The format.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static ITexture FromArray<TYPE>(TYPE[,] data, PixelInternalFormat internalFormat, PixelFormat format, PixelType type)
		{
			GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				IntPtr pointer = pinnedArray.AddrOfPinnedObject();
				var width = data.GetLength(0);
				var height = data.GetLength(1);
				var texture = new Texture2dGL
				{
					Filter = TextureFilterMode.Mipmap
				};
				texture.Activate();
				texture.LoadPixels(pointer, width, height, internalFormat, format, type);
				texture.Deactivate();
				return texture;
			}
			finally
			{
				pinnedArray.Free();
			}
		}

		/// <summary>
		/// Froms the bitmap.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		public static ITexture FromBitmap(Bitmap bitmap)
		{
			var texture = new Texture2dGL
			{
				Filter = TextureFilterMode.Mipmap
			};
			texture.Activate();
			//todo: 16bit channels
			using (Bitmap bmp = new Bitmap(bitmap))
			{
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), SysDraw.ImageLockMode.ReadOnly, bmp.PixelFormat);
				var internalFormat = SelectInternalPixelFormat(bmp.PixelFormat);
				var inputPixelFormat = SelectPixelFormat(bmp.PixelFormat);
				texture.LoadPixels(bmpData.Scan0, bmpData.Width, bmpData.Height, internalFormat, inputPixelFormat, PixelType.UnsignedByte);
				bmp.UnlockBits(bmpData);
			}
			texture.Deactivate();
			return texture;
		}

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
		/// Froms the file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="FileLoadException"></exception>
		public static ITexture FromFile(string fileName)
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
		/// Saves a texture to a buffer.
		/// </summary>
		/// <param name="texture">The texture to save.</param>
		/// <param name="buffer">The buffer to write to</param>
		public static void ToBuffer(this ITexture2D texture, ref Vector4[] buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));

			try
			{
				texture.Activate();
				GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.Float, buffer);
				texture.Deactivate();
			}
			catch
			{
				texture.Deactivate();
			}
		}

		/// <summary>
		/// Selects the pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="FileLoadException">Wrong pixel format " + pixelFormat.ToString()</exception>
		public static PixelFormat SelectPixelFormat(SysDraw.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case SysDraw.PixelFormat.Format8bppIndexed: return PixelFormat.Red;
				case SysDraw.PixelFormat.Format24bppRgb: return PixelFormat.Bgr;
				case SysDraw.PixelFormat.Format32bppArgb: return PixelFormat.Bgra;
				default: throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
			}
		}

		/// <summary>
		/// Selects the internal pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="FileLoadException">Wrong pixel format " + pixelFormat.ToString()</exception>
		public static PixelInternalFormat SelectInternalPixelFormat(SysDraw.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case SysDraw.PixelFormat.Format8bppIndexed: return PixelInternalFormat.Luminance;
				case SysDraw.PixelFormat.Format24bppRgb: return PixelInternalFormat.Rgb;
				case SysDraw.PixelFormat.Format32bppArgb: return PixelInternalFormat.Rgba;
				default: throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
			}
		}

		/// <summary>
		/// Selects the internal pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="FileLoadException">Wrong pixel format " + pixelFormat.ToString()</exception>
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
				return PixelInternalFormat.Luminance;
			}
			else throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
		}

		/// <summary>
		/// Selects the pixel format.
		/// </summary>
		/// <param name="pixelFormat">The pixel format.</param>
		/// <returns></returns>
		/// <exception cref="FileLoadException">Wrong pixel format " + pixelFormat.ToString()</exception>
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
			else throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
		}
	}
}
