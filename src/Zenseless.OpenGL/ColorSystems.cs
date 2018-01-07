using Zenseless.Geometry;
using System.Numerics;
using SysColor = System.Drawing.Color;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	public static class ColorSystems
	{
		/// <summary>
		/// Converts hsb (Hue, Saturation and Brightness) color value into rgb
		/// </summary>
		/// <param name="h">Hue [0..1]</param>
		/// <param name="s">Saturation [0..1]</param>
		/// <param name="b">Brightness [0..1]</param>
		/// <returns>
		/// rgb color
		/// </returns>
		public static Vector3 Hsb2rgb(float h, float s, float b)
		{
			s = MathHelper.Clamp(s, 0, 1);
			b = MathHelper.Clamp(b, 0, 1);
			var v3 = new Vector3(3.0f);
			var i = h * 6.0f;
			var j = new Vector3(i, i + 4.0f, i + 2.0f).Mod(6.0f);
			var k = Vector3.Abs(j - v3);
			var l = k - Vector3.One;
			var rgb = l.Clamp(0.0f, 1.0f);
			return b * Vector3.Lerp(Vector3.One, rgb, s);
		}

		//todo: transformation not correct
		//public static Vector3 Color2Hsb(SysColor color)
		//{
		//	return new Vector3(color.GetHue() / 360f, color.GetSaturation(), color.GetBrightness());
		//}

		/// <summary>
		/// To the color of the system.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		public static SysColor ToSystemColor(this Vector3 color)
		{
			color *= 255;
			return SysColor.FromArgb((int)color.X, (int)color.Y, (int)color.Z);
		}

		/// <summary>
		/// To the vector3.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		public static Vector3 ToVector3(this SysColor color)
		{
			return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
		}
	}
}
