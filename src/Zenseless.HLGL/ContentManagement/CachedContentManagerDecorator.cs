namespace Zenseless.HLGL
{
	using System;
	using System.Collections.Generic;
	using Zenseless.Patterns;

	/// <summary>
	/// 
	/// </summary>
	public class CachedContentManagerDecorator : IContentManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CachedContentManagerDecorator"/> class.
		/// </summary>
		/// <param name="contentManager">The content manager.</param>
		public CachedContentManagerDecorator(IContentManager contentManager)
		{
			ContentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
		}

		/// <summary>
		/// Gets the decorated content manager.
		/// </summary>
		/// <value>
		/// The content manager.
		/// </value>
		public IContentManager ContentManager { get; private set; }

		/// <summary>
		/// Occurs after a new cache entry was created.
		/// </summary>
		public event EventHandler<NewCacheEntryEventArgs> NewCacheEntry;

		/// <summary>
		/// Gets a list of registered importer types.
		/// </summary>
		/// <value>
		/// The importer types.
		/// </value>
		public IEnumerable<Type> ImporterTypes => ContentManager.ImporterTypes;

		/// <summary>
		/// Enumerates all content resource names.
		/// </summary>
		/// <value>
		/// All content resource names.
		/// </value>
		public IEnumerable<string> Names => ContentManager.Names;

		/// <summary>
		/// Disposes all loaded content instances.
		/// </summary>
		public void DisposeInstances()
		{
			foreach(var instance in _instanceCache.Values)
			{
				if (instance is IDisposable disposable)
				{
					disposable.Dispose();
				}
				_instanceCache.Clear();
			}
		}

		/// <summary>
		/// Creates an instance of a given type from the resources with the specified names.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="names">A list of resource names.</param>
		/// <returns>
		/// An instance of the given type.
		/// </returns>
		public TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class
		{
			var name = string.Join(";", names);
			if (_instanceCache.TryGetValue(name, out var instance))
			{
				if (instance is not TYPE typedInstance) throw new ArgumentException($"Wrong type '{typeof(TYPE).FullName}' for instance of type '{instance.GetType().FullName}'.");
				return typedInstance;
			}
			var fullNames = Names.GetFullNames(names);
			var result = ContentManager.Load<TYPE>(fullNames);
			_instanceCache[name] = result;
			NewCacheEntry?.Invoke(this, new NewCacheEntryEventArgs(result, name, fullNames));
			return result;
		}

		/// <summary>
		/// Registers an importer.
		/// </summary>
		/// <typeparam name="TYPE">The return type of the importer.</typeparam>
		/// <param name="importer">The importer instance.</param>
		public void RegisterImporter<TYPE>(Func<IEnumerable<NamedStream>, TYPE> importer) where TYPE : class
		{
			ContentManager.RegisterImporter(importer);
		}

		private readonly Dictionary<string, object> _instanceCache = new Dictionary<string, object>();
	}
}
