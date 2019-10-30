namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;

	/// <summary>
	/// 
	/// </summary>
	public static class TextureLoaderImaging
	{
		//public static ITexture FromStream(Stream stream) //TODO: not working
		//{
		//	var source = new BitmapImage();
		//	source.BeginInit();
		//	source.StreamSource = stream;
		//	source.EndInit();
		//	//TODO: flip bitmap image
		//	var writable = new WriteableBitmap(source);
		//	writable.Lock();
		//	var internalFormat = SelectInternalPixelFormat(source.Format);
		//	var inputPixelFormat = SelectPixelFormat(source.Format);
		//	var texture = new Texture2dGL();
		//	texture.LoadPixels(writable.BackBuffer, source.PixelWidth, source.PixelHeight, internalFormat, inputPixelFormat, PixelType.UnsignedByte);
		//	writable.Unlock();
		//	texture.Filter = TextureFilterMode.Mipmap;
		//	return texture;
		//}

		//private static PixelInternalFormat SelectInternalPixelFormat(SysMedia.PixelFormat pixelFormat)
		//{
		//	if (SysMedia.PixelFormats.Bgra32 == pixelFormat)
		//	{
		//		return PixelInternalFormat.Rgba;
		//	}
		//	else if (SysMedia.PixelFormats.Rgb24 == pixelFormat)
		//	{
		//		return PixelInternalFormat.Rgb;
		//	}
		//	else if (SysMedia.PixelFormats.Gray8 == pixelFormat)
		//	{
		//		return PixelInternalFormat.R8;
		//	}
		//	else throw new ArgumentException("Wrong pixel format " + pixelFormat.ToString());
		//}

		//private static PixelFormat SelectPixelFormat(SysMedia.PixelFormat pixelFormat)
		//{
		//	if (SysMedia.PixelFormats.Bgra32 == pixelFormat)
		//	{
		//		return PixelFormat.Bgra;
		//	}
		//	else if (SysMedia.PixelFormats.Rgb24 == pixelFormat)
		//	{
		//		return PixelFormat.Bgr;
		//	}
		//	else if (SysMedia.PixelFormats.Gray8 == pixelFormat)
		//	{
		//		return PixelFormat.Red;
		//	}
		//	else throw new ArgumentException("Wrong pixel format " + pixelFormat.ToString());
		//}
	}
}
