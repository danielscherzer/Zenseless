using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Contains methods for accessing the frame buffer 
	/// </summary>
	public static class FrameBuffer
	{
		/// <summary>
		/// Do the necessary rotate and flip for GL buffer to <seealso cref="Bitmap"/> conversion.
		/// </summary>
		/// <param name="bitmap">The bitmap that will be rotated and flipped</param>
		public static void RotateFlip(this Bitmap bitmap)
		{
			bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
		}

		/// <summary>
		/// Saves a rectangular area of the current frame buffer into a Bitmap
		/// </summary>
		/// <param name="x">start position in x-direction</param>
		/// <param name="y">start position in y-direction</param>
		/// <param name="width">size in x-direction</param>
		/// <param name="height">size in y-direction</param>
		/// <param name="rotateFlip">If <code>true</code> image will be rotated and flipped, which is correct,
		/// but this is a time-consuming operation and should be switched off for fast screen capturing.
		/// <code>true</code> by default.</param>
		/// <returns>Bitmap</returns>
		public static Bitmap ToBitmap(int x, int y, int width, int height, bool rotateFlip = true)
		{
			var format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
			var bmp = new Bitmap(width, height);
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, format);
			GL.ReadPixels(x, y, width, height, TextureLoaderDrawing.SelectPixelFormat(format), PixelType.UnsignedByte, data.Scan0);
			bmp.UnlockBits(data);
			if(rotateFlip) bmp.RotateFlip();
			return bmp;
		}

		/// <summary>
		/// Saves the contents of the current frame buffer into a Bitmap
		/// </summary>
		/// <param name="rotateFlip">If <code>true</code> image will be rotated and flipped, which is correct,
		/// but this is a time-consuming operation and should be switched off for fast screen capturing.
		/// <code>true</code> by default.</param>
		/// <returns>Bitmap</returns>
		public static Bitmap ToBitmap(bool rotateFlip = true)
		{
			var viewport = new int[4];
			GL.GetInteger(GetPName.Viewport, viewport);
			return ToBitmap(viewport[0], viewport[1], viewport[2], viewport[3], rotateFlip);
		}
	}
}
