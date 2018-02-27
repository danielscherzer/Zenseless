using System;
using System.Collections.Generic;
using System.IO;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="INamedStreamLoader" />
	public class FileLoader : INamedStreamLoader
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
		/// Tries to return the file path for a given name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="filePath">The file path</param>
		/// <returns><c>true</c> if a filePath is available.</returns>
		public bool TryGetPath(string name, out string filePath) => mappings.TryGetValue(name, out filePath);

		private Dictionary<string, string> mappings = new Dictionary<string, string>();
	}
}
