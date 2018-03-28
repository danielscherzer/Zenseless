using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class IntersectionExtensionsTests
	{
		[TestMethod()]
		public void IntersectsTest()
		{
			var box = new Box2D(0, 0, 1, 1);
			var circle = new Circle(1, 1, 1);
			Assert.IsTrue(box.Intersects(circle));
		}

		[TestMethod()]
		public void IntersectsTest2()
		{
			var box = new Box2D(0, 0, 1, 1);
			var circle = new Circle(2, 1, 1);
			Assert.IsFalse(box.Intersects(circle));
		}

		[TestMethod()]
		public void IntersectsTest3()
		{
			var box = new Box2D(0, 0, 1, 1);
			var circle = new Circle(0, 2, 1);
			Assert.IsFalse(box.Intersects(circle));
		}

		[TestMethod()]
		public void IntersectsTest4()
		{
			var box = new Box2D(0, 0, 1, 1);
			var circle = new Circle(2, 2, 1);
			Assert.IsFalse(box.Intersects(circle));
		}

		[TestMethod()]
		public void IntersectsTest5()
		{
			var box = new Box2D(0, 0, 1, 1);
			var circle = new Circle(1.5f, 1.5f, 1);
			Assert.IsTrue(box.Intersects(circle));
		}

		[TestMethod()]
		public void IntersectsTest6()
		{
			var box = new Box2D(0, 0, 1, 1);
			var circle = new Circle(-0.5f, -0.5f, 1);
			Assert.IsTrue(box.Intersects(circle));
		}
	}
}