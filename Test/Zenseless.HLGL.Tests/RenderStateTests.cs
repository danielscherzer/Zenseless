using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zenseless.HLGL.Tests
{
	using static States;

	[TestClass()]
	public class RenderStateTests
	{
		[TestMethod()]
		public void RegisterTest()
		{
			var renderState = new RenderState();
			renderState.Register(null, BoolState<IDepthState>.Disabled);
			Assert.IsFalse(renderState.Get<BoolState<IDepthState>>().IsEnabled);
			renderState.Set(BoolState<IDepthState>.Enabled);
			Assert.IsTrue(renderState.Get<BoolState<IDepthState>>().IsEnabled);
			renderState.Set(BoolState<IDepthState>.Disabled);
			Assert.IsFalse(renderState.Get<BoolState<IDepthState>>().IsEnabled);
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void UnregisteredTest()
		{
			var renderState = new RenderState();
			var depthTest = renderState.Get<BoolState<IDepthState>>().IsEnabled;
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterWrongTypeTest()
		{
			var renderState = new RenderState();
			renderState.Register(null, BoolState<IDepthState>.Disabled);
			var depthTest = renderState.Get<BoolState<IPointSpriteState>>().IsEnabled;
		}
	}
}