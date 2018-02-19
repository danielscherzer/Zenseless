using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Interface for a loader of named streams.
	/// </summary>
	public interface INamedStreamLoader
	{
		/// <summary>
		/// Enumerates all stream names.
		/// </summary>
		/// <value>
		/// All stream names.
		/// </value>		
		IEnumerable<string> Names { get; }

		/// <summary>
		/// Gets the stream with the given name.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <returns>A <seealso cref="NamedStream"/>.</returns>
		NamedStream GetStream(string name);
	}
}