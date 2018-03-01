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
		/// Adds a stream creator.
		/// </summary>
		/// <param name="name">The name of the functor.</param>
		/// <param name="streamCreator">A functor for creating a stream.</param>
		/// <exception cref="ArgumentException"></exception>
		public void AddStreamCreator(string name, Func<Stream> streamCreator)
		{
			if (streamCreator == null) throw new ArgumentNullException(nameof(streamCreator));
			namedCreators.Add(name, streamCreator);
		}

		/// <summary>
		/// Determines whether the specified name is known.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		///   <c>true</c> if the specified name is known; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string name) => namedCreators.ContainsKey(name);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public NamedStream CreateStream(string name)
		{
			if (namedCreators.TryGetValue(name, out var createStream))
			{
				var stream = createStream.Invoke();
				return new NamedStream(name, stream);
			}
			throw new ArgumentException($"No creator for '{name}' was found.");
		}

		private Dictionary<string, Func<Stream>> namedCreators = new Dictionary<string, Func<Stream>>();

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<string> Names => namedCreators.Keys;
	}
}
