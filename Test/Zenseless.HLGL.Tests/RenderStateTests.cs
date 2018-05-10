using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zenseless.HLGL.Tests
{
	[TestClass()]
	public class RenderStateTests
	{
		[TestMethod()]
		public void RegisterTest()
		{
			var renderState = new RenderState();
			renderState.Register(null, new DepthTest(false));
			Assert.IsFalse(renderState.Get<DepthTest>().Enabled);
			renderState.Set(new DepthTest(true));
			Assert.IsTrue(renderState.Get<DepthTest>().Enabled);
			renderState.Set(new DepthTest(false));
			Assert.IsFalse(renderState.Get<DepthTest>().Enabled);
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void UnregisteredTest()
		{
			var renderState = new RenderState();
			var depthTest = renderState.Get<DepthTest>().Enabled;
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterWrongTypeTest()
		{
			var renderState = new RenderState();
			renderState.Register(null, new DepthTest(false));
			var depthTest = renderState.Get<PointSprite>().Enabled;
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void SetWrongTypeTest()
		{
			var renderState = new RenderState();
			renderState.Register(null, new DepthTest(false));
			renderState.Set(new PointSprite(true));
		}
	}
}