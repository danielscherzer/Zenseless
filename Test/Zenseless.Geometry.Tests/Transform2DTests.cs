using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class Transform2DTests
	{
		[TestMethod()]
		public void RotateAroundOriginTest90()
		{
			var a = Vector2.UnitX;
			var t = new Transformation2D();
			t.RotateLocal(90f);
			var expectedA = Vector2.UnitY;
			Assert.AreEqual(expectedA, t.Transform(a));
		}

		[TestMethod()]
		public void RotateAroundOriginTest180()
		{
			var a = Vector2.UnitX;
			var t = new Transformation2D();
			t.RotateLocal(180f);
			var expectedA = -a;
			Assert.AreEqual(expectedA, t.Transform(a));
		}

		[TestMethod()]
		public void RotateAroundOriginTestIdentity()
		{
			var t = new Transformation2D();
			t.RotateLocal(360f);
			var expectedM = Matrix3x2.Identity;
			Assert.AreEqual(expectedM, t);
		}

		[TestMethod()]
		public void RotateAroundOriginTestIdentity2()
		{
			var t = new Transformation2D();
			t.RotateLocal(0f);
			var expectedM = Matrix3x2.Identity;
			Assert.AreEqual(expectedM, t);
		}

		[TestMethod()]
		public void RotateAroundTest()
		{
			var a = Vector2.Zero;
			var t = Transformation2D.CreateRotationAround(new Vector2(-1, -1), 90f);
			var expectedA = -2 * Vector2.UnitX;
			Assert.AreEqual(expectedA, t.Transform(a));
		}

		[TestMethod()]
		public void ScaleAroundOriginTest()
		{
			var a = new Vector2(-3, 2);
			var t = new Transformation2D();
			t.ScaleLocal(2, -3);
			var expectedA = new Vector2(-6, -6);
			Assert.AreEqual(expectedA, t.Transform(a));
		}

		[TestMethod()]
		public void ScaleAroundTest()
		{
			var a = new Vector2(1, 1);
			var pivot = new Vector2(-1, -1);
			var t = Transformation2D.CreateScaleAround(pivot, 2, 3);
			var expectedA = new Vector2(3, 5);
			Assert.AreEqual(expectedA, t.Transform(a));
		}
	}
}