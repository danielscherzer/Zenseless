using System;
using System.Collections.Generic;
using System.IO;
using Zenseless.Patterns;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="INamedStreamLoader" />
	public class FileLoader : Disposable, INamedStreamLoader
	{
		/// <summary>
		/// Enumerates all stream names.
		/// </summary>
		/// <value>
		/// All stream names.
		/// </value>
		public IEnumerable<string> Names => mappings.Keys;

		/// <summary>
		/// Adds the mapping.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="filePath">The file path.</param>
		/// <exception cref="ArgumentException"></exception>
		public void AddMapping(string name, string filePath)
		{
			if (!File.Exists(filePath)) throw new ArgumentException($"The file {filePath} was not found.");
			mappings.Add(name, filePath);
		}

		/// <summary>
		/// Determines whether the specified name is known.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		///   <c>true</c> if the specified name is known; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string name) => mappings.ContainsKey(name);

		/// <summary>
		/// Gets the stream with the given name.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <returns>
		/// A <seealso cref="NamedStream" />.
		/// </returns>
		/// <exception cref="ArgumentException"></exception>
		public NamedStream CreateStream(string name)
		{
			if (mappings.TryGetValue(name, out var filePath))
			{
				var stream = new FileStream(filePath, FileMode.Open);
				return new NamedStream(name, stream);
			}
			throw new ArgumentException($"The name mapping '{name}' was not found.");
		}

		/// <summary>
		/// Gets the file path.
		/// </summary>
		/// <param name="fullName">The full name.</param>
		/// <returns><seealso cref="string.Empty"/> if no file path was found.</returns>
		public string GetFilePath(string fullName)
		{
			if (mappings.TryGetValue(fullName, out var filePath))
			{
				return filePath;
			}
			return string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DisposeResources()
		{
			foreach (var watcher in watchers) watcher.Value.Dispose();
		}

		private Dictionary<string, string> mappings = new Dictionary<string, string>();
		private Dictionary<string, FileWatcher> watchers = new Dictionary<string, FileWatcher>();

		internal void InstallWatcher(string fullName, Action<string> onChange)
		{
			if (onChange == null) throw new ArgumentNullException(nameof(onChange));
			if (mappings.TryGetValue(fullName, out var filePath))
			{
				if (!watchers.TryGetValue(filePath, out var watcher))
				{
					watcher = new FileWatcher(filePath);
					watchers.Add(filePath, watcher);
				}
				watcher.Changed += (s, a) => onChange.Invoke(filePath);
			}
		}
	}
}
