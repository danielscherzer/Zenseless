using System;
using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Class for event argument for the NewCacheEntry event
	/// </summary>
	/// <seealso cref="EventArgs" />
	public class NewCacheEntryEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NewCacheEntryEventArgs"/> class.
		/// </summary>
		/// <param name="name">The name of the cache entry.</param>
		/// <param name="names"></param>
		/// <param name="instance"></param>
		/// <exception cref="ArgumentNullException">name</exception>
		public NewCacheEntryEventArgs(object instance, string name, IEnumerable<string> names)
		{
			Instance = instance ?? throw new ArgumentNullException(nameof(instance));
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Names = names ?? throw new ArgumentNullException(nameof(names));
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public object Instance { get; }
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; }

		/// <summary>
		/// Gets the names.
		/// </summary>
		/// <value>
		/// The names.
		/// </value>
		public IEnumerable<string> Names { get; }
	}
}