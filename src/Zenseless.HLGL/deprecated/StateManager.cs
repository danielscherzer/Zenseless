using System;
using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IStateManager" />
	public class StateManager : IStateManager
	{
		/// <summary>
		/// Returns the state implementation registered with the KEYTYPE cast to the given INTERFACE
		/// </summary>
		/// <typeparam name="INTERFACE">interface you want the returned state implementation to have</typeparam>
		/// <typeparam name="KEYTYPE">used to determine which registered state implementation to return</typeparam>
		/// <returns>a state implementation</returns>
		public INTERFACE Get<INTERFACE, KEYTYPE>() where KEYTYPE : IState where INTERFACE : class, IState
		{
			var type = typeof(KEYTYPE);
			if (states.TryGetValue(type, out IState stateImplementation))
			{
				return (INTERFACE)stateImplementation;
			}
			throw new ArgumentException("State '" + type.Name + "' not registered.");
		}

		/// <summary>
		/// Register a state implementation with the unique key type KEYTYPE and the access interface INTERFACE
		/// </summary>
		/// <typeparam name="INTERFACE">interface intended for later GetState calls, here used for sanity type checking</typeparam>
		/// <typeparam name="KEYTYPE">unique key type</typeparam>
		/// <param name="stateImplementation">implementation of INTERFACE</param>
		public void Register<INTERFACE, KEYTYPE>(IState stateImplementation) where INTERFACE : IState where KEYTYPE : INTERFACE
		{
			SanityChecks<INTERFACE, KEYTYPE>(stateImplementation);
			states.Add(typeof(KEYTYPE), stateImplementation);
		}

		private Dictionary<Type, IState> states = new Dictionary<Type, IState>();

		private void SanityChecks<INTERFACE, KEYTYPE>(object implementation)
		{
			var intrfc = typeof(INTERFACE);
			var type = implementation.GetType();
			if (!intrfc.IsAssignableFrom(type))
			{
				throw new InvalidCastException("State implementation '" + type.Name + "' cannot be assigned to interface '" + intrfc.Name + "'");
			}
			var keyType = typeof(KEYTYPE);
			if (states.ContainsKey(keyType)) throw new ArgumentException("IDTYPE '" + keyType.Name + "' already registered");
		}
	}
}
