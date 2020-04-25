using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
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
				var texture = new Texture2dGL();
				texture.LoadPixels(pointer, width, height, internalFormat, format, type);
				texture.Filter = TextureFilterMode.Mipmap;
				return texture;
			}
			finally
			{
				pinnedArray.Free();
			}
		}

		/// <summary>
		/// Saves a texture to a buffer.
		/// </summary>
		/// <param name="texture">The texture to save.</param>
		/// <param name="buffer">The buffer to write to</param>
		public static void ToBuffer(this ITexture2D texture, ref Vector4[] buffer)
		{
			if (buffer is null) throw new ArgumentNullException(nameof(buffer));

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
		/// Saves a texture to a buffer.
		/// </summary>
		/// <param name="texture">The texture to save.</param>
		/// <param name="buffer">The buffer to write to</param>
		public static void ToBuffer(this ITexture2D texture, ref float[] buffer)
		{
			if (buffer is null) throw new ArgumentNullException(nameof(buffer));

			try
			{
				texture.Activate();
				GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Red, PixelType.Float, buffer);
				texture.Deactivate();
			}
			catch
			{
				texture.Deactivate();
			}
		}
	}
}
