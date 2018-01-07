using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class CircleTests
	{
		[TestMethod()]
		public void IntersectsCircleTest()
		{
			var a = new Circle(0, 0, 1);
			var b = new Circle(1, 0, 1);
			Assert.IsTrue(a.Intersects(b));
		}

		[TestMethod()]
		public void IntersectsCircleTest2()
		{
			var a = new Circle(-1, 0, 1);
			var b = new Circle(1, 0, 1);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void IntersectsCircleTest3()
		{
			var a = new Circle(0, 0, 0.99f);
			var b = new Circle(2, 0, 1);
			Assert.IsFalse(a.Intersects(b));
		}
	}
}