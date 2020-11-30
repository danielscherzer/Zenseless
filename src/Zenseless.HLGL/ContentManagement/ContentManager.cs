using System;
using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Implementation of a content manager
	/// </summary>
	/// <seealso cref="IContentManager" />
	public class ContentManager : IContentManager //TODO: check if we can decompose this class loader, importer... answer if the content manager should be split up into services (import, update services) or be a single service 
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
		/// Gets a list of registered importer types.
		/// </summary>
		/// <value>
		/// The importer types.
		/// </value>
		public IEnumerable<Type> ImporterTypes => _importers.Keys;

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
			_importers.Add(typeof(TYPE), importer);
		}
		
		/// <summary>
		/// Creates an instance of a given type from the specified names.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="names">A list of names.</param>
		/// <returns>
		/// An instance of the given type if an importer for the TYPE is registered.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// </exception>
		public TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class
		{
			var type = typeof(TYPE);
			if (_importers.TryGetValue(type, out var importer))
			{
				var namedStreams = new List<NamedStream>();
				foreach (var name in names)
				{
					var fullName = Names.GetFullName(name); //TODO: ?put name resolution only in extension methods
					namedStreams.Add(loader.Open(fullName));
				}
				if (0 == namedStreams.Count) throw new ArgumentException("No input resources given!");
				var result = importer(namedStreams) as TYPE;
				namedStreams.Dispose();
				return result;
			}
			throw new ArgumentException($"No converter for type '{type.FullName}' was found.");
		}

		private readonly INamedStreamLoader loader;
		private readonly Dictionary<Type, Func<IEnumerable<NamedStream>, object>> _importers = new Dictionary<Type, Func<IEnumerable<NamedStream>, object>>();
	}
}
