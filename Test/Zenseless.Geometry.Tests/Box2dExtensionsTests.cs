using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Zenseless.Geometry.Tests
{
	[TestClass()]
	public class Box2dExtensionsTests
	{
		[TestMethod()]
		public void TransformCenterTestTranslate()
		{
			var a = new Box2D(0, 0, 2, 4);
			var m = Matrix3x2.CreateTranslation(-1, 1);
			var expectedA = new Box2D(-1, 1, a.SizeX, a.SizeY);
			a.TransformCenter(m);
			Assert.AreEqual(expectedA, a);
		}

		[TestMethod()]
		public void TransformCenterTestNull()
		{
			var a = new Box2D(-1, -2, 2, 4);
			var m = new Matrix3x2();
			var expectedA = new Box2D(a);
			a.TransformCenter(m);
			Assert.AreEqual(expectedA, a);
		}

		[TestMethod()]
		public void TransformCenterTestIdentity()
		{
			var a = new Box2D(1, -2, 3, 4);
			var m = Matrix3x2.Identity;
			var expectedA = new Box2D(a);
			a.TransformCenter(m);
			Assert.AreEqual(expectedA, a);
		}

		[TestMethod()]
		public void TransformCenterTestScale()
		{
			var a = new Box2D(-1, -2, 2, 4);
			var m = Matrix3x2.CreateScale(3);
			var expectedA = new Box2D(a);
			a.TransformCenter(m);
			Assert.AreEqual(expectedA, a);
		}

		[TestMethod()]
		public void TransformCenterTestScale2()
		{
			var a = new Box2D(0, 0, 2, 4);
			var m = Matrix3x2.CreateScale(3);
			var expectedA = new Box2D(2, 4, a.SizeX, a.SizeY);
			a.TransformCenter(m);
			Assert.AreEqual(expectedA, a);
		}

		[TestMethod()]
		public void UndoOverlapTestNoOverlap()
		{
			var a = new Box2D(-1, -3, 1, 2);
			var b = new Box2D(0, -1, 1, 1);
			var expectedA = new Box2D(a);
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void UndoOverlapTest2()
		{
			var aX = -1f;
			var a = new Box2D(aX + 0.1f, 0, 2, 2);
			var b = new Box2D(1, 0, 2, 2);
			a.UndoOverlap(b);
			Assert.AreEqual(aX, a.MinX);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void UndoOverlapTest3()
		{
			var a = new Box2D(0, 0, 0, 0);
			var b = new Box2D(a);
			a.UndoOverlap(b);
			Assert.AreEqual(b, a);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void UndoOverlapTest4()
		{
			var a = new Box2D(-1, -1, 2, 3);
			var b = new Box2D(a);
			var expectedA = new Box2D(a);
			expectedA.MinX += 2f;
			a.MinX += 0.01f; //to make it unambiguous in which direction to push
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void UndoOverlapTest5()
		{
			var a = new Box2D(-1, -1, 3, 2);
			var b = new Box2D(a);
			var expectedA = new Box2D(a);
			expectedA.MinY += 2f;
			a.MinY += 0.01f; //to make it unambiguous in which direction to push
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void UndoOverlapTest6()
		{
			var a = new Box2D(-1, -1, 2, 3);
			var b = new Box2D(a);
			var expectedA = new Box2D(a);
			expectedA.MinX -= 2f;
			a.MinX -= 0.01f; //to make it unambiguous in which direction to push
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void UndoOverlapTest7()
		{
			var a = new Box2D(-1, -1, 3, 2);
			var b = new Box2D(a);
			var expectedA = new Box2D(a);
			expectedA.MinY = -3f;
			a.MinY -= 0.01f; //to make it unambiguous in which direction to push
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsFalse(a.Intersects(b));
		}
		[TestMethod()]
		public void UndoOverlapTest8()
		{
			var a = new Box2D(-1, -1, 3, 2);
			var b = new Box2D(-4, -4, 10, 10);
			var expectedA = new Box2D(a);
			expectedA.MinY = -6f;
			a.UndoOverlap(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsFalse(a.Intersects(b));
		}

		[TestMethod()]
		public void PushXRangeInsideTest()
		{
			var a = new Box2D(-0.1f, 0, 0.5f, 0.5f);
			var b = new Box2D(0, 0, 2, 2);
			var expectedA = new Box2D(a);
			expectedA.MinX = 0;
			a.PushXRangeInside(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsTrue(a.Intersects(b));
		}

		[TestMethod()]
		public void PushXRangeInsideTest2()
		{
			var a = new Box2D(1.6f, 0, 0.5f, 0.5f);
			var b = new Box2D(0, 0, 2, 2);
			var expectedA = new Box2D(a);
			expectedA.MinX = 1.5f;
			a.PushXRangeInside(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsTrue(a.Intersects(b));
		}

		[TestMethod()]
		public void PushYRangeInsideTest()
		{
			var a = new Box2D(0, -0.1f, 0.5f, 0.5f);
			var b = new Box2D(0, 0, 2, 2);
			var expectedA = new Box2D(a);
			expectedA.MinY = 0;
			a.PushYRangeInside(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsTrue(a.Intersects(b));
		}

		[TestMethod()]
		public void PushYRangeInsideTest2()
		{
			var a = new Box2D(0, 1.6f, 0.5f, 0.5f);
			var b = new Box2D(0, 0, 2, 2);
			var expectedA = new Box2D(a);
			expectedA.MinY = 1.5f;
			a.PushYRangeInside(b);
			Assert.AreEqual(expectedA, a);
			Assert.IsTrue(a.Intersects(b));
		}

		[TestMethod()]
		public void CreateContainingBoxSquareTest()
		{
			var width = 10f;
			var height = 10f;
			var aspect = width / height;
			var fitBox = Box2DExtensions.CreateContainingBox(width, height, aspect);
			Assert.AreEqual(new Box2D(0, 0, width, height), fitBox);
		}

		[TestMethod()]
		public void CreateContainingBoxPortraitTest()
		{
			PortraitTest(1f, 3f, 3f / 3f);
			PortraitTest(2f, 3f, 3f / 3f);
			PortraitTest(3f, 3f, 3f / 3f);
			PortraitTest(3f, 3f, 8f / 3f);
			PortraitTest(4f, 3f, 8f / 3f);
			PortraitTest(5f, 3f, 8f / 3f);
			PortraitTest(6f, 3f, 8f / 3f);
			PortraitTest(7f, 3f, 8f / 3f);
			PortraitTest(8f, 3f, 8f / 3f);
		}

		[TestMethod()]
		public void CreateContainingBoxLandscapeTest()
		{
			LandscapeTest(3f, 3f, 3f / 3f);
			LandscapeTest(3f, 2f, 3f / 3f);
			LandscapeTest(3f, 1f, 3f / 3f);
		}

		private static void PortraitTest(float width, float height, float aspect)
		{
			var fitBox = Box2DExtensions.CreateContainingBox(width, height, aspect);
			var delta = .0001f;
			Assert.AreEqual(-.5f * (aspect * height - width), fitBox.MinX, delta);
			Assert.AreEqual(0f, fitBox.MinY, delta);
			Assert.AreEqual(height * aspect, fitBox.SizeX, delta);
			Assert.AreEqual(height, fitBox.SizeY, delta);
		}

		private static void LandscapeTest(float width, float height, float aspect)
		{
			var fitBox = Box2DExtensions.CreateContainingBox(width, height, aspect);
			var delta = .0001f;
			Assert.AreEqual(0f, fitBox.MinX, delta);
			Assert.AreEqual(-.5f * (aspect * width - height), fitBox.MinY, delta);
			Assert.AreEqual(width, fitBox.SizeX, delta);
			Assert.AreEqual(width / aspect, fitBox.SizeY, delta);
		}
	}
}