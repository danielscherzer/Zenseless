using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Zenseless.Base;

namespace Zenseless.HLGL
{
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
		/// <param name="resourceAssembly"></param>
		public FileContentManager(Assembly resourceAssembly)
		{
			var streamLoader = new StreamLoader();
			namedStreamLoader = streamLoader;
			AddMappings(streamLoader, resourceAssembly);
			cachedContentManager = new CachedContentManagerDecorator(new ContentManager(namedStreamLoader));
			cachedContentManager.NewCacheEntry += FileContentManagerDecorator_NewCacheEntry;
		}

		private static void AddMappings(StreamLoader streamLoader, Assembly resourceAssembly)
		{
			var resourceNames = resourceAssembly.GetManifestResourceNames();

			foreach(var name in resourceNames)
			{
				Stream CreateStream() => resourceAssembly.GetManifestResourceStream(name);
				streamLoader.AddMapping(name, CreateStream);
			}
		}

		/// <summary>
		/// Gets the last changed file path.
		/// </summary>
		/// <value>
		/// The last changed file path.
		/// </value>
		public string LastChangedFilePath { get; private set; }

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
				LastChangedFilePath = fileChangeData.FilePath;
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
		public void RegisterUpdater<TYPE>(Action<TYPE, IEnumerable<NamedStream>> updater) where TYPE : class
		{
			if (updater is null) throw new ArgumentNullException($"The updater must not be null.");
			updaters.Add(typeof(TYPE), (i, res) => updater(i as TYPE, res));
		}

		/// <summary>
		/// Sets the content search directory. 
		/// This is needed if you want to do automatic runtime content reloading if the content source file changes. 
		/// This feature is disabled otherwise. The execution time of this command is dependent on how many files are found inside the given directory.
		/// </summary>
		/// <param name="contentSearchDirectory">The content search directory. Content is found in this directory or subdirectories</param>		
		public void SetContentSearchDirectory(string contentSearchDirectory)
		{
			var mapping = Names.FindFiles(contentSearchDirectory);
			fileLoader = new FileLoader();
			foreach (var watcher in watchers) watcher.Value.Dispose();
			watchers.Clear();
			foreach (var entry in mapping)
			{
				fileLoader.AddMapping(entry.Key, entry.Value);
			};
		}

		private struct FileChangeData
		{
			public FileChangeData(string filePath, object instance, IEnumerable<string> names)
			{
				FilePath = filePath;
				Instance = instance;
				Names = names;
			}

			public string FilePath { get; }
			public object Instance { get; }
			public IEnumerable<string> Names { get; }
		}

		private ConcurrentQueue<FileChangeData> changedFiles = new ConcurrentQueue<FileChangeData>();
		private FileLoader fileLoader;
		private Dictionary<Type, Action<object, IEnumerable<NamedStream>>> updaters = new Dictionary<Type, Action<object, IEnumerable<NamedStream>>>();
		private Dictionary<string, FileWatcher> watchers = new Dictionary<string, FileWatcher>();
		private readonly INamedStreamLoader namedStreamLoader;
		private readonly CachedContentManagerDecorator cachedContentManager;

		private void FileContentManagerDecorator_NewCacheEntry(object sender, NewCacheEntryEventArgs e)
		{
			if (fileLoader is null) return;
			//create a file watcher for each resource
			foreach (var fullName in e.Names)
			{
				if (fileLoader.TryGetPath(fullName, out var filePath))
				{
					var watcher = GetWatcher(filePath);
					watcher.Changed += (s, a) => changedFiles.Enqueue(new FileChangeData(filePath, e.Instance, e.Names));
				}
			}
		}

		private IEnumerable<NamedStream> OpenStreams(IEnumerable<string> names)
		{
			var namedStreams = new List<NamedStream>();
			foreach (var name in names)
			{
				try
				{
					if (fileLoader.Contains(name))
					{
						namedStreams.Add(fileLoader.CreateStream(name));
					}
					else
					{
						namedStreams.Add(namedStreamLoader.CreateStream(name));
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

		private FileWatcher GetWatcher(string filePath)
		{
			if(!watchers.TryGetValue(filePath, out var watcher))
			{
				watcher = new FileWatcher(filePath);
				watchers.Add(filePath, watcher);
			}
			return watcher;
		}
	}
}
