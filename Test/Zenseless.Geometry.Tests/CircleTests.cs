using Zenseless.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class CircleTests
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
			Assert.IsFalse(b.Intersects(a));
		}

		[TestMethod()]
		public void IntersectsCircleTest3()
		{
			var a = new Circle(0, 0, 0.99f);
			var b = new Circle(2, 0, 1);
			Assert.IsFalse(a.Intersects(b));
			Assert.IsFalse(b.Intersects(a));
		}

		[TestMethod()]
		public void IntersectsCircleTest4()
		{
			var a = new Circle(0, 0, 0.1f);
			var b = new Circle(0.1f, 0.1f, 0.1f);
			Assert.IsTrue(a.Intersects(b));
			Assert.IsTrue(b.Intersects(a));
		}

		[TestMethod()]
		public void IntersectsCircleTest5()
		{
			var a = new Circle(0, 0, 0.1f);
			var b = new Circle(0.2f, 0.1f, 0.1f);
			Assert.IsFalse(a.Intersects(b));
			Assert.IsFalse(b.Intersects(a));
		}

		[TestMethod()]
		public void ContainsTest()
		{
			var circle = new Circle(0, 0, 1f);
			var point = new Vector2(0.1f);
			Assert.IsTrue(circle.Contains(point));
		}
	}
}