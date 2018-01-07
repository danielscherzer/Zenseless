using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class MathHelperTests
	{
		[TestMethod()]
		public void ToPolarTest()
		{
			TestToPolar(0, 0, 0, 0);
			TestToPolar(1, 0, 0, 1);
			TestToPolar(0, 1, 0.5 * Math.PI, 1); //90°
			TestToPolar(0, 4, 0.5 * Math.PI, 4); //90°
			TestToPolar(-1, 0, Math.PI, 1); //180°
			TestToPolar(-2, 0, Math.PI, 2); //180°
			TestToPolar(0, -1, -1.0 / 2.0 * Math.PI, 1); //270°
			TestToPolar(0, -2, -1.0 / 2.0 * Math.PI, 2); // 270°
		}

		private static void TestToPolar(double inputX, double inputY, double expectedX, double expectedY)
		{
			var input = new Vector2((float)inputX, (float)inputY);
			var expected = new Vector2((float)expectedX, (float)expectedY);
			Assert.AreEqual(expected, MathHelper.ToPolar(input));
		}

		[TestMethod()]
		public void PackUnorm4x8Test()
		{
			for (uint x = 0; x < 256; x += 5)
			{
				for (uint y = 0; y < 256; y += 5)
				{
					for (uint z = 0; z < 256; z += 5)
					{
						for (uint w = 0; w < 256; w += 5)
						{
							var input = new Vector4(x, y, z, w);
							var output = MathHelper.UnpackUnorm4x8(MathHelper.PackUnorm4x8(input / 255f));
							output *= 255f;
							var delta = .0001f;
							Assert.AreEqual(input.X, output.X, delta);
							Assert.AreEqual(input.Y, output.Y, delta);
							Assert.AreEqual(input.Z, output.Z, delta);
							Assert.AreEqual(input.W, output.W, delta);
						}
					}
				}
			}
		}
	}
}