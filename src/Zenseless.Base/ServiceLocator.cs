using System;
using System.Collections.Generic;

namespace Zenseless.Base //TODO: rename base -> patterns
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceLocator //TODO: answer if the content manager should be split up into services (import, update services) or be a single service 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TYPE"></typeparam>
		/// <returns></returns>
		public TYPE GetService<TYPE>() where TYPE : class
		{
			var type = typeof(TYPE);
			if (services.TryGetValue(type, out var instance))
			{
				return (TYPE)instance;
			}
			throw new ArgumentException($"No service of type '{type.FullName}' was found.");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TYPE"></typeparam>
		/// <param name="instance"></param>
		public void RegisterService<TYPE>(TYPE instance) where TYPE : class
		{
			if (instance is null) throw new ArgumentNullException(nameof(instance));
			services.Add(typeof(TYPE), instance);
		}

		private Dictionary<Type, object> services = new Dictionary<Type, object>();
	}
}
