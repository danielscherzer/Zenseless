using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Zenseless.OpenGL.Tests
{
	[TestClass()]
	public class ColorSystemsTests
	{
		[TestMethod()]
		public void Hsb2rgb()
		{
			Hsb2rgb(0, 0, 0, Color.Black);
			Hsb2rgb(1, 0, 0, Color.Black);
			Hsb2rgb(1, 1, 0, Color.Black);
			Hsb2rgb(0, 0, 1, Color.White);
			Hsb2rgb(0, 1, 1, Color.Red);
			Hsb2rgb(1f / 6f, 1, 1, Color.Yellow);
			Hsb2rgb(2f / 6f, 1, 1, Color.Lime); //0,1,0
			Hsb2rgb(3f / 6f, 1, 1, Color.Cyan);
			Hsb2rgb(4f / 6f, 1, 1, Color.Blue);
			Hsb2rgb(5f / 6f, 1, 1, Color.Magenta);
			Hsb2rgb(0, 0, 128f / 255f, Color.Gray);
		}

		public void Hsb2rgb(float h, float s, float b, Color expectedColor)
		{
			var rgb = ColorSystems.Hsb2rgb(h, s, b);
			var expected = expectedColor.ToVector3();
			Assert.AreEqual(expected, rgb);
		}
	}
}