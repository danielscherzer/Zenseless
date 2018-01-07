using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class CircleExtensionsTests
	{
		[TestMethod()]
		public void UndoOverlapTest()
		{
			var a = new Circle(0, 0, 1);
			var b = new Circle(1, 0, 1);
			var expectedA = new Circle(-1, 0, 1);
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
		}

		[TestMethod()]
		public void UndoOverlapTest2()
		{
			var a = new Circle(0, 0, 1);
			var b = new Circle(1, 1, 1);
			var delta = 1 - (float)Math.Sqrt(2);
			var expectedA = new Circle(delta, delta, 1);
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
		}
	}
}