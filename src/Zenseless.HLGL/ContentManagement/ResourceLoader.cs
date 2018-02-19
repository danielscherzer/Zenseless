using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Loads streams form embedded resources
	/// </summary>
	/// <seealso cref="INamedStreamLoader" />
	public class ResourceLoader : INamedStreamLoader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceLoader"/> class.
		/// </summary>
		/// <param name="resourceAssembly">The resource assembly.</param>
		public ResourceLoader(Assembly resourceAssembly)
		{
			this.resourceAssembly = resourceAssembly;
			resourceNames = resourceAssembly.GetManifestResourceNames();
		}

		/// <summary>
		/// Enumerates all stream names.
		/// </summary>
		/// <value>
		/// All stream names.
		/// </value>
		public IEnumerable<string> Names => resourceNames;

		/// <summary>
		/// Gets the stream with the given name.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <returns>
		/// A <seealso cref="NamedStream" />.
		/// </returns>
		/// <exception cref="ArgumentException"></exception>
		public NamedStream GetStream(string name)
		{
			if (!resourceNames.Contains(name)) throw new ArgumentException($"The embedded resource '{name}' was not found.");
			return new NamedStream(name, resourceAssembly.GetManifestResourceStream(name));
		}

		private readonly Assembly resourceAssembly;
		private readonly IEnumerable<string> resourceNames;
	}
}
