namespace Zenseless.HLGL
{
	using System;
	using System.Linq;
	using System.Collections.Concurrent;
	using System.Collections.Generic;

	/// <summary>
	/// A content manager for file and resource based content management. 
	/// If a file is available for a resource, the file will be loaded and if changed during run-time the content is recreated
	/// </summary>
	/// <seealso cref="CachedContentManagerDecorator" />
	public class FileContentManager : IContentManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileContentManager"/> class.
		/// </summary>
		/// <param name="loader"></param>
		public FileContentManager(INamedStreamLoader loader)
		{
			namedStreamLoader = loader ?? throw new ArgumentNullException(nameof(loader));
			cachedContentManager = new CachedContentManagerDecorator(new ContentManager(namedStreamLoader));
			cachedContentManager.NewCacheEntry += FileContentManagerDecorator_NewCacheEntry;
		}

		/// <summary>
		/// Updater delegate
		/// </summary>
		/// <typeparam name="TYPE">The type of the instance.</typeparam>
		/// <param name="instance">The instance.</param>
		/// <param name="resources">The resources to load into the instance.</param>
		public delegate void Updater<TYPE>(TYPE instance, IEnumerable<NamedStream> resources);

		/// <summary>
		/// Gets a list of registered importer types.
		/// </summary>
		/// <value>
		/// The importer types.
		/// </value>
		public IEnumerable<Type> ImporterTypes => cachedContentManager.ImporterTypes;

		/// <summary>
		/// Enumerates all content resource names.
		/// </summary>
		/// <value>
		/// All content resource names.
		/// </value>
		public IEnumerable<string> Names => namedStreamLoader.Names;

		/// <summary>
		/// Checks for resource change.
		/// </summary>
		public void CheckForResourceChange()
		{
			if(changedFiles.TryDequeue(out var fileChangeData))
			{
				var instance = fileChangeData.Instance;
				var type = instance.GetType();
				if (updaters.TryGetValue(type, out var updater))
				{
					var namedStreams = OpenStreams(fileChangeData.Names);
					//check for stream access errors and if so try it later again
					if (namedStreams is null)
					{
						changedFiles.Enqueue(fileChangeData);
						return;
					}
					try
					{
						updater(instance, namedStreams);
					}
					finally
					{
						namedStreams.Dispose();
					}
				}
			}
		}

		/// <summary>
		/// Gets the file path.
		/// </summary>
		/// <param name="fullName">The full name.</param>
		/// <returns><seealso cref="string.Empty"/> if no file path was found.</returns>
		public string GetFilePath(string fullName)
		{
			if (fileLoader is null) return fullName;
			return fileLoader.GetFilePath(fullName);
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
			return cachedContentManager.Load<TYPE>(names);
		}

		/// <summary>
		/// Registers an importer.
		/// </summary>
		/// <typeparam name="TYPE">The return type of the importer.</typeparam>
		/// <param name="importer">The importer instance.</param>
		public void RegisterImporter<TYPE>(Func<IEnumerable<NamedStream>, TYPE> importer) where TYPE : class
		{
			cachedContentManager.RegisterImporter(importer);
		}

		/// <summary>
		/// Registers the updater.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <param name="updater">The updater.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void RegisterUpdater<TYPE>(Updater<TYPE> updater) where TYPE : class
		{
			if (updater is null) throw new ArgumentNullException($"The updater must not be null.");
			var type = typeof(TYPE);
			bool AssignableFrom(Type storedType) => storedType.IsAssignableFrom(type);
			if(!cachedContentManager.ImporterTypes.Any(AssignableFrom)) throw new ArgumentException($"No importer with a type assignable from '{type.Name}' was found.");
			updaters.Add(type, (i, res) => updater(i as TYPE, res));
		}

		/// <summary>
		/// Sets the content search directory. 
		/// This is needed if you want to do automatic runtime content reloading if the content source file changes. 
		/// This feature is disabled otherwise. The execution time of this command is dependent on how many files are found inside the given directory.
		/// </summary>
		/// <param name="contentSearchDirectory">The content search directory. Content is found in this directory or subdirectories</param>		
		public void SetContentSearchDirectory(string contentSearchDirectory)
		{
			if (!(fileLoader is null)) fileLoader.Dispose();
			if (contentSearchDirectory is null) return;
			var mapping = Names.FindFiles(contentSearchDirectory);
			fileLoader = new FileLoader();
			foreach (var entry in mapping)
			{
				fileLoader.AddMapping(entry.Key, entry.Value);
			};
		}

		private struct ChangedData
		{
			public ChangedData(object instance, IEnumerable<string> names)
			{
				Instance = instance;
				Names = names;
			}

			public object Instance { get; }
			public IEnumerable<string> Names { get; }
		}

		private ConcurrentQueue<ChangedData> changedFiles = new ConcurrentQueue<ChangedData>();
		private FileLoader fileLoader;
		private Dictionary<Type, Action<object, IEnumerable<NamedStream>>> updaters = new Dictionary<Type, Action<object, IEnumerable<NamedStream>>>();
		private readonly INamedStreamLoader namedStreamLoader;
		private readonly CachedContentManagerDecorator cachedContentManager;

		private void FileContentManagerDecorator_NewCacheEntry(object sender, NewCacheEntryEventArgs e)
		{
			if (fileLoader is null) return;
			//create a file watcher for each resource
			foreach (var fullName in e.Names)
			{
				void OnChange(string filePath) => changedFiles.Enqueue(new ChangedData(e.Instance, e.Names));
				fileLoader.InstallWatcher(fullName, OnChange);
			}
		}

		private IEnumerable<NamedStream> OpenStreams(IEnumerable<string> names)
		{
			var namedStreams = new List<NamedStream>();
			foreach (var name in names)
			{
				try
				{
					if (fileLoader.Exists(name))
					{
						namedStreams.Add(fileLoader.Open(name));
					}
					else
					{
						namedStreams.Add(namedStreamLoader.Open(name));
					}
				}
				catch
				{
					namedStreams.Dispose(); // cleanup
					return null; // notify of error
				}
			}
			return namedStreams;
		}
	}
}
