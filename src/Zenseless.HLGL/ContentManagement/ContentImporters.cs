namespace Zenseless.HLGL
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using Zenseless.Geometry;

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
		public static StringBuilder StringBuilder(IEnumerable<NamedStream> resources)
		{
			StringBuilder output = new StringBuilder();
			Update(output, resources);
			return output;
		}


		/// <summary>
		/// Updates the specified output.
		/// </summary>
		/// <param name="output">The output.</param>
		/// <param name="resources">The resources.</param>
		public static void Update(StringBuilder output, IEnumerable<NamedStream> resources)
		{
			output.Clear();
			foreach (var res in resources)
			{
				using (var reader = new StreamReader(res.Stream, true))
				{
					output.Append(reader.ReadToEnd());
				}
			}
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

		/// <summary>
		/// Meshes the specified resources.
		/// </summary>
		/// <param name="resources">The resources.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">No elements</exception>
		public static DefaultMesh DefaultMesh(IEnumerable<NamedStream> resources)
		{
			return Obj2Mesh.FromObj(ByteBuffer(resources));
		}
	}
}
