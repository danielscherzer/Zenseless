using System;
using System.Collections.Generic;
using System.Text;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public class CachedContentManagerDecorator : IContentManager
	{
		private readonly IContentManager contentManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="CachedContentManagerDecorator"/> class.
		/// </summary>
		/// <param name="contentManager">The content manager.</param>
		public CachedContentManagerDecorator(IContentManager contentManager)
		{
			if (contentManager is null) throw new ArgumentException($"Parameter '{nameof(contentManager)}' must not be null");
			this.contentManager = contentManager;
		}

		/// <summary>
		/// Enumerates all content resource names.
		/// </summary>
		/// <value>
		/// All content resource names.
		/// </value>
		public IEnumerable<string> Names => contentManager.Names;

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
			var name = Combine(names);
			if (instanceCache.TryGetValue(name, out var instance))
			{
				var typedInstance = instance as TYPE;
				if(typedInstance is null) throw new ArgumentException($"Wrong type '{typeof(TYPE).FullName}' for instance of type '{instance.GetType().FullName}'.");
				return typedInstance;
			}
			var result = contentManager.Load<TYPE>(names);
			instanceCache[name] = result;
			return result;
		}

		/// <summary>
		/// Registers an importer.
		/// </summary>
		/// <typeparam name="TYPE">The return type of the importer.</typeparam>
		/// <param name="importer">The importer instance.</param>
		public void RegisterImporter<TYPE>(Func<IEnumerable<NamedStream>, TYPE> importer) where TYPE : class
		{
			contentManager.RegisterImporter(importer);
		}

		private Dictionary<string, object> instanceCache = new Dictionary<string, object>();

		private string Combine(IEnumerable<string> names)
		{
			var result = new StringBuilder();
			foreach(var name in names)
			{
				result.Append(name);
			}
			return result.ToString();
		}
	}
}
