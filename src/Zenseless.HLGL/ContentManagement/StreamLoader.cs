using System;
using System.Collections.Generic;
using System.IO;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Class that implements a general loader for named streams
	/// </summary>
	public class StreamLoader : INamedStreamLoader
	{
		/// <summary>
		/// Adds the mapping.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="streamCreator"></param>
		/// <exception cref="ArgumentException"></exception>
		public void AddMapping(string name, Func<Stream> streamCreator)
		{
			if (streamCreator == null) throw new ArgumentNullException(nameof(streamCreator));
			mappings.Add(name, streamCreator);
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
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public NamedStream CreateStream(string name)
		{
			if (mappings.TryGetValue(name, out var createStream))
			{
				var stream = createStream.Invoke();
				return new NamedStream(name, stream);
			}
			throw new ArgumentException($"The name mapping '{name}' was not found.");
		}

		private Dictionary<string, Func<Stream>> mappings = new Dictionary<string, Func<Stream>>();

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<string> Names => mappings.Keys;
	}
}
