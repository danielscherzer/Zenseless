using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Implementation of a content manager for embedded resources
	/// </summary>
	/// <seealso cref="IContentLoader" />
	public class ResourceContentManager : IContentManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceContentManager"/> class.
		/// </summary>
		/// <param name="resourceAssembly">The resource assembly.</param>
		public ResourceContentManager(Assembly resourceAssembly)
		{
			this.resourceAssembly = resourceAssembly;
			resourceNames = resourceAssembly.GetManifestResourceNames();
		}

		/// <summary>
		/// Enumerates all content resource keys.
		/// </summary>
		/// <value>
		/// All content resource keys.
		/// </value>
		public IEnumerable<string> Names => resourceNames;

		/// <summary>
		/// Registers an importer.
		/// </summary>
		/// <typeparam name="TYPE">The return type of the importer.</typeparam>
		/// <param name="importer">The importer instance.</param>
		/// <exception cref="ArgumentException"></exception>
		public void RegisterImporter<TYPE>(Func<IEnumerable<NamedStream>, TYPE> importer) where TYPE : class
		{
			if (importer is null) throw new ArgumentException($"The importer must not be null.");
			importers.Add(typeof(TYPE), importer);
		}
		
		/// <summary>
		/// Creates an instance of a given type from the resources with the specified keys.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="keys">A list of resource keys.</param>
		/// <returns>
		/// An instance of the given type if an importer for the TYPE is registered.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// </exception>
		public TYPE Load<TYPE>(IEnumerable<string> keys) where TYPE : class
		{
			var resources = new List<NamedStream>();
			foreach (var name in keys)
			{ 
				var fullName = this.GetFullName(name);
				if (fullName is null) throw new ArgumentException($"The embedded resource '{name}' was not found.");
				resources.Add(new NamedStream(fullName, resourceAssembly.GetManifestResourceStream(fullName)));
			}
			var type = typeof(TYPE);
			if (importers.TryGetValue(type, out var importer))
			{
				var result = importer.Invoke(resources) as TYPE;
				foreach (var res in resources) res.Stream.Dispose();
				return result;
			}
			throw new ArgumentException($"No converter for type '{type.FullName}' was found.");
		}

		private readonly IEnumerable<string> resourceNames;
		private readonly Assembly resourceAssembly;
		private Dictionary<Type, Func<IEnumerable<NamedStream>, object>> importers = new Dictionary<Type, Func<IEnumerable<NamedStream>, object>>();
	}
}
