using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Interface for a loader of named streams.
	/// </summary>
	public interface INamedStreamLoader
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		bool Contains(string name);

		/// <summary>
		/// Enumerates all stream names.
		/// </summary>
		/// <value>
		/// All stream names.
		/// </value>		
		IEnumerable<string> Names { get; }

		/// <summary>
		/// Creates a stream with the given name.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <returns>A <seealso cref="NamedStream"/>.</returns>
		NamedStream CreateStream(string name);
	}
}