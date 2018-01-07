using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class Box2DTests
	{
		[TestMethod()]
		[ExpectedException(typeof(System.NullReferenceException), "A Box2D of null was inappropriately allowed.")]
		public void IntersectsTestNull()
		{
			var a = new Box2D(0, 0, 1, 1);
			var expectedA = new Box2D(a);
			a.Intersects(null);
		}

		[TestMethod()]
		public void IntersectsTestNone()
		{
			var a = new Box2D(0, 0, 1, 1);
			var expectedA = new Box2D(a);
			var b = new Box2D(5, 5, 1, 1);
			var expectedB = new Box2D(b);
			Assert.IsFalse(a.Intersects(b));
			Assert.AreEqual(a, expectedA);
			Assert.AreEqual(b, expectedB);
		}

		[TestMethod()]
		public void IntersectsTestNone2()
		{
			var a = new Box2D(-4, -7, 10, 20);
			var b = new Box2D(6, -7, 10, 20);
			Assert.IsFalse(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsTestNone3()
		{
			var a = new Box2D(-4, -7, 10, 20);
			var b = new Box2D(-4, 13, 10, 20);
			Assert.IsFalse(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsBoxTest()
		{
			var a = new Box2D(-4, -7, 1, 2);
			var b = new Box2D(a);
			Assert.IsTrue(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsBoxTest2()
		{
			var a = new Box2D(-4, -7, 1, 2);
			var b = new Box2D(a);
			b.MinX += b.SizeX - 0.001f;
			Assert.IsTrue(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}

		[TestMethod()]
		public void IntersectsBoxTest3()
		{
			var a = new Box2D(-4, -7, 1, 2);
			var b = new Box2D(a);
			b.MinY += b.SizeY - 0.001f;
			Assert.IsTrue(a.Intersects(b));
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
		}
	}
}