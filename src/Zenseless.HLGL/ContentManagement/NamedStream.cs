using System.IO;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public struct NamedStream
	{
		/// <summary>
		/// The name
		/// </summary>
		public readonly string Name;
		/// <summary>
		/// The stream
		/// </summary>
		public readonly Stream Stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="NamedStream"/> struct.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="stream">The stream.</param>
		public NamedStream(string name, Stream stream)
		{
			Name = name;
			Stream = stream;
		}
	}
}
