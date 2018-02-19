using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Zenseless.Base;

namespace Zenseless.HLGL
{
	/// <summary>
	/// A content manager for file and resource based content management. 
	/// If a file is available for a resource, the file will be loaded and if changed during run-time the content is recreated
	/// </summary>
	/// <seealso cref="CachedContentManagerDecorator" />
	public class FileContentManager : CachedContentManagerDecorator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileContentManager"/> class.
		/// </summary>
		/// <param name="resourceAssembly"></param>
		public FileContentManager(Assembly resourceAssembly) : base(new ContentManager(new ResourceLoader(resourceAssembly)))
		{
			NewCacheEntry += FileContentManagerDecorator_NewCacheEntry;
		}
		
		/// <summary>
		/// Checks for resource change.
		/// </summary>
		public void CheckForResourceChange()
		{
			if(changedInstances.TryDequeue(out var instanceData))
			{
				var instance = instanceData.Instance;
				var type = instance.GetType();
				if (updaters.TryGetValue(type, out var updater))
				{
					var namedStreams = OpenStreams(instanceData.Names);
					//check for stream access errors and if so try it later again
					if (namedStreams is null)
					{
						changedInstances.Enqueue(instanceData);
						return;
					}
					updater(instance, namedStreams);
					namedStreams.Dispose();
				}
			}
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
			var mapping = Loader.ResolveNamedStreamFiles(contentSearchDirectory);
			fileLoader = new FileLoader();
			foreach (var watcher in watchers) watcher.Value.Dispose();
			watchers.Clear();
			foreach (var entry in mapping)
			{
				fileLoader.AddMapping(entry.Key, entry.Value);
			};
		}

		private struct InstanceData
		{
			public InstanceData(object instance, IEnumerable<string> names)
			{
				Instance = instance;
				Names = names;
			}

			public object Instance { get; }
			public IEnumerable<string> Names { get; }
		}

		private ConcurrentQueue<InstanceData> changedInstances = new ConcurrentQueue<InstanceData>();
		private FileLoader fileLoader;
		private Dictionary<Type, Action<object, IEnumerable<NamedStream>>> updaters = new Dictionary<Type, Action<object, IEnumerable<NamedStream>>>();
		private Dictionary<string, FileWatcher> watchers = new Dictionary<string, FileWatcher>();

		private void FileContentManagerDecorator_NewCacheEntry(object sender, NewCacheEntryEventArgs e)
		{
			if (fileLoader is null) return;
			//create a file watcher for each resource
			foreach (var fullName in e.Names)
			{
				if (fileLoader.TryGetPath(fullName, out var filePath))
				{
					var watcher = GetWatcher(filePath);
					watcher.Changed += (s, a) => changedInstances.Enqueue(new InstanceData(e.Instance, e.Names));
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
						namedStreams.Add(fileLoader.GetStream(name));
					}
					else
					{
						namedStreams.Add(Loader.GetStream(name));
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
