namespace Zenseless.HLGL
{
	using System.Collections.Generic;

	/// <summary>
	/// 
	/// </summary>
	public class NamedStreamLoaderCollection : INamedStreamLoader
	{
		/// <summary>
		/// If a finite set of stream names are known these are enumerated (like for a resource assembly), null otherwise (like for a file system).
		/// </summary>
		/// <value>
		/// List of names or null;
		/// </value>
		public IEnumerable<string> Names
		{
			get
			{
				foreach (var loader in loaders)
				{
					foreach (var name in loader.Names)
					{
						yield return name;
					}
				}
			}
		}

		/// <summary>
		/// Adds the specified loader.
		/// </summary>
		/// <param name="loader">The loader.</param>
		/// <param name="highPriority">if set to <c>true</c> [high priority].</param>
		public void Add(INamedStreamLoader loader, bool highPriority = false)
		{
			if (highPriority)
			{
				loaders.Insert(0, loader);
			}
			else
			{
				loaders.Add(loader);
			}
		}

		/// <summary>
		/// Check if the specified name exists.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public bool Exists(string name)
		{
			foreach (var loader in loaders)
			{
				if (loader.Exists(name)) return true;
			}
			return false;
		}

		/// <summary>
		/// Opens a stream with the given name.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <returns>
		/// A <seealso cref="NamedStream" />.
		/// </returns>
		public NamedStream Open(string name)
		{
			foreach(var loader in loaders)
			{
				if(loader.Exists(name))
				{
					var namedStream = loader.Open(name);
					return namedStream;
				}
			}
			return null;
		}

		private List<INamedStreamLoader> loaders = new List<INamedStreamLoader>();
	}
}
