using System;
using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Implementation of a content manager
	/// </summary>
	/// <seealso cref="IContentManager" />
	public class ContentManager : IContentManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentManager"/> class.
		/// </summary>
		/// <param name="loader">The loader.</param>
		/// <exception cref="ArgumentNullException">loader</exception>
		public ContentManager(INamedStreamLoader loader)
		{
			this.loader = loader ?? throw new ArgumentNullException(nameof(loader));
		}

		/// <summary>
		/// Enumerates all content resource names.
		/// </summary>
		/// <value>
		/// All content resource names.
		/// </value>
		public IEnumerable<string> Names => loader.Names;

		/// <summary>
		/// Registers an importer.
		/// </summary>
		/// <typeparam name="TYPE">The return type of the importer.</typeparam>
		/// <param name="importer">The importer instance.</param>
		/// <exception cref="ArgumentException"></exception>
		public void RegisterImporter<TYPE>(Func<IEnumerable<NamedStream>, TYPE> importer) where TYPE : class
		{
			if (importer is null) throw new ArgumentNullException($"The importer must not be null.");
			importers.Add(typeof(TYPE), importer);
		}
		
		/// <summary>
		/// Creates an instance of a given type from the resources with the specified keys.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="names">A list of resource names.</param>
		/// <returns>
		/// An instance of the given type if an importer for the TYPE is registered.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// </exception>
		public TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class
		{
			var type = typeof(TYPE);
			if (importers.TryGetValue(type, out var importer))
			{
				var namedStreams = new List<NamedStream>();
				foreach (var name in names)
				{
					var fullName = Names.GetFullName(name);
					namedStreams.Add(loader.CreateStream(fullName));
				}
				if (0 == namedStreams.Count) throw new ArgumentException("No input resources given!");
				var result = importer.Invoke(namedStreams) as TYPE;
				namedStreams.Dispose();
				return result;
			}
			throw new ArgumentException($"No converter for type '{type.FullName}' was found.");
		}

		private readonly INamedStreamLoader loader;
		private Dictionary<Type, Func<IEnumerable<NamedStream>, object>> importers = new Dictionary<Type, Func<IEnumerable<NamedStream>, object>>();
	}
}
