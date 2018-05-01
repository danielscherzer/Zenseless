namespace Zenseless.OpenGL
{
	using System.Numerics;
	using Zenseless.Geometry;
	using SysColor = System.Drawing.Color;

	/// <summary>
	/// Class for color system transformations
	/// </summary>
	public static class ColorSystems
	{
		/// <summary>
		/// Converts HSB (Hue, Saturation and Brightness) color value into RGB
		/// </summary>
		/// <param name="hue">Hue [0..1]</param>
		/// <param name="saturation">Saturation [0..1]</param>
		/// <param name="brightness">Brightness [0..1]</param>
		/// <returns>
		/// RGB color
		/// </returns>
		public static Vector3 Hsb2rgb(float hue, float saturation, float brightness)
		{
			saturation = MathHelper.Clamp(saturation, 0f, 1f);
			brightness = MathHelper.Clamp(brightness, 0f, 1f);
			var v3 = new Vector3(3f);
			var i = hue * 6f;
			var j = new Vector3(i, i + 4f, i + 2f).Mod(6f);
			var k = Vector3.Abs(j - v3);
			var l = k - Vector3.One;
			var rgb = l.Clamp(0f, 1f);
			return brightness * Vector3.Lerp(Vector3.One, rgb, saturation);
		}

		//TODO: transformation not correct
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
