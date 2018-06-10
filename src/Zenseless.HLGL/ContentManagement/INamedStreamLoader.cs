namespace Zenseless.HLGL
{
	using System.Collections.Generic;

	/// <summary>
	/// Interface for a creator of named streams
	/// </summary>
	public interface INamedStreamLoader
	{
		/// <summary>
		/// If a finite set of stream names are known these are enumerated (like for a resource assembly), otherwise (like for a file system) an empty set is returned.
		/// </summary>
		/// <value>
		/// List of names or null;
		/// </value>		
		IEnumerable<string> Names { get; }

		/// <summary>
		/// Opens a stream with the given name.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <returns>A <seealso cref="NamedStream"/>.</returns>
		NamedStream Open(string name);

		/// <summary>
		/// Check if the specified name exists.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		bool Exists(string name);
	}
}