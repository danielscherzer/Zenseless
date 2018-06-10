namespace Zenseless.HLGL
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	/// <summary>
	/// Handles named file streams
	/// </summary>
	/// <seealso cref="INamedStreamLoader" />
	public class NamedFileStreamLoader : INamedStreamLoader
	{
		/// <summary>
		/// If a finite set of stream names are known these are enumerated (like for a resource assembly), otherwise (like for a file system) an empty set is returned.
		/// </summary>
		/// <value>
		/// List of names or null;
		/// </value>
		public IEnumerable<string> Names => Enumerable.Empty<string>();

		/// <summary>
		/// Opens a file stream with the given file path.
		/// </summary>
		/// <param name="name">The file path of the stream.</param>
		/// <returns>
		/// A <seealso cref="NamedStream" />.
		/// </returns>
		public NamedStream Open(string name)
		{
			var stream = new FileStream(name, FileMode.Open);
			return new NamedStream(name, stream);
		}

		/// <summary>
		/// Check if the specified name exists.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public bool Exists(string name)
		{
			return File.Exists(name);
		}
	}
}
