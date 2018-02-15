using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Some often used content importers
	/// </summary>
	public static class ContentImporters
	{
		/// <summary>
		/// Creates a string out of a list of streams.
		/// </summary>
		/// <param name="resources">The import resources.</param>
		/// <returns>A string.</returns>
		public static string String(IEnumerable<NamedStream> resources)
		{
			var sb = new StringBuilder();
			foreach (var res in resources)
			{
				using (var reader = new StreamReader(res.Stream, true))
				{
					sb.Append(reader.ReadToEnd());
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Creates a byte array out of a list of streams.
		/// </summary>
		/// <param name="resources">The import resources.</param>
		/// <returns>A byte array.</returns>
		/// <exception cref="ArgumentException">No elements</exception>
		public static byte[] ByteBuffer(IEnumerable<NamedStream> resources)
		{
			foreach (var res in resources)
			{
				using (BinaryReader br = new BinaryReader(res.Stream))
				{
					return br.ReadBytes((int)res.Stream.Length);
				}
			}
			throw new ArgumentException("No elements");
		}
	}
}
