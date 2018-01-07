using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zenseless.HLGL.Tests
{
	using static States;

	[TestClass()]
	public class StateManagerTests
	{
		class StateBool : IStateBool
		{
			public bool Enabled { get; set; } = false;
		}

		class StateFloat : IStateTyped<float>
		{
			public float Value { get; set; }
		}

		[TestMethod()]
		public void RegisterTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBlending>(new StateBool());
			var blend = stateManager.Get<IStateBool, IBlending>();
			blend.Enabled = true;
			Assert.IsTrue(stateManager.Get<IStateBool, IBlending>().Enabled);

			blend.Enabled = false;
			Assert.IsFalse(stateManager.Get<IStateBool, IBlending>().Enabled);
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void UnregisteredTest()
		{
			var stateManager = new StateManager();
			var blend = stateManager.Get<IStateBool, IBlending>();
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterWrongTypeTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBackfaceCulling>(new StateBool());
			var state = stateManager.Get<IStateBool, IBlending>();
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidCastException))]
		public void RegisterWrongInterfaceTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBlending>(new StateFloat());
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidCastException))]
		public void RegisterWrongInterfaceOnGetTest()
		{
			var stateManager = new StateManager();
			stateManager.Register<IStateBool, IBlending>(new StateFloat());
			var state = stateManager.Get<IStateBool, IBlending>();
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterSameTest()
		{
			var stateManager = new StateManager();
			//not intended purpsoe but still valid
			stateManager.Register<StateBool, StateBool>(new StateBool());
			//register same again
			stateManager.Register<StateBool, StateBool>(new StateBool());
			var state = stateManager.Get<IStateBool, StateBool>();
		}

		class StateBool2 : StateBool { };

		[TestMethod()]
		public void RegisterCustomClassTest()
		{
			var stateManager = new StateManager();
			//not intended purpose but still valid
			stateManager.Register<StateBool, StateBool>(new StateBool());
			//register same again
			stateManager.Register<StateBool, StateBool2>(new StateBool());
			var state = stateManager.Get<IStateBool, StateBool>();
			var state2 = stateManager.Get<IStateBool, StateBool2>();
			Assert.AreNotSame(state, state2);
			Assert.AreEqual(state.Enabled, state2.Enabled);
		}
	}
}