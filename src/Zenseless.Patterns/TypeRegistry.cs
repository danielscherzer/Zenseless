using System;
using System.Collections.Generic;

namespace Zenseless.Patterns
{
	/// <summary>
	/// Holds a dictionary of type instances that can be registered and requested.
	/// </summary>
	public class TypeRegistry
	{
		/// <summary>
		/// Register an instance of a unique type with this type registry
		/// </summary>
		/// <typeparam name="TYPE">An unique type</typeparam>
		/// <param name="instance">An instance</param>
		public void RegisterTypeInstance<TYPE>(TYPE instance) where TYPE : class
		{
			if (instance is null) throw new ArgumentNullException(nameof(instance));
			types.Add(typeof(TYPE), instance);
		}

		/// <summary>
		/// Returns the registered instance of the given type.
		/// </summary>
		/// <typeparam name="TYPE">An unique type</typeparam>
		/// <returns>An instance of the given type</returns>
		public TYPE GetInstance<TYPE>() where TYPE : class
		{
			var type = typeof(TYPE);
			if (types.TryGetValue(type, out var instance))
			{
				return (TYPE)instance;
			}
			return null;
		}

		private readonly Dictionary<Type, object> types = new Dictionary<Type, object>();
	}
}
