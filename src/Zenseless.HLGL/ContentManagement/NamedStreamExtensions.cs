using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Extension methods for The <seealso cref="NamedStream"/> class
	/// </summary>
	public static class NamedStreamExtensions
	{
		/// <summary>
		/// Disposes the list of named streams.
		/// </summary>
		/// <param name="namedStreams">The named streams.</param>
		public static void Dispose(this IEnumerable<NamedStream> namedStreams)
		{
			foreach (var namedStream in namedStreams) namedStream.Stream.Dispose();
		}

	}
}
