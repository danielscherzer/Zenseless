using System;
using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public class RenderState : IRenderState
	{
		/// <summary>
		/// Gets this instance.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public TYPE Get<TYPE>() where TYPE : IEquatable<TYPE>
		{
			var type = typeof(TYPE);
			if (states.TryGetValue(type, out Data data))
			{
				return (TYPE)data.Value;
			}
			throw new ArgumentException(TypeNotRegisteredMessage(type));
		}

		/// <summary>
		/// Registers the specified update.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <param name="update">The update.</param>
		/// <param name="defaultValue"></param>
		public void Register<TYPE>(Action<TYPE> update, TYPE defaultValue) where TYPE : IEquatable<TYPE>
		{
			//TODO: decide what to do if the type is already registered? currently the old registration is overwritten
			states.Add(typeof(TYPE), new Data(update, defaultValue));
		}

		/// <summary>
		/// Sets the specified value.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <param name="value">The value.</param>
		/// <exception cref="ArgumentException"></exception>
		public void Set<TYPE>(TYPE value) where TYPE : IEquatable<TYPE>
		{
			var type = typeof(TYPE);
			if (states.TryGetValue(type, out Data data))
			{
				if (value.Equals(data.Value)) return; //do nothing if same value
				data.Value = value;
				var update = data.UpdateHandler as Action<TYPE>;
				update?.Invoke(value);
			}
			else throw new ArgumentException(TypeNotRegisteredMessage(type));
		}

		private class Data
		{
			public object UpdateHandler, Value;

			public Data(object updateHandler, object value)
			{
				UpdateHandler = updateHandler;
				Value = value;
			}
		};

		private Dictionary<Type, Data> states = new Dictionary<Type, Data>();

		private static string TypeNotRegisteredMessage(Type type)
		{
			return $"State '{type}' not registered.";
		}
	}
}
