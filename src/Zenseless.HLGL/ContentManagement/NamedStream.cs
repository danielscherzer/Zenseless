using System;
using System.IO;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public struct NamedStream
	{
		/// <summary>
		/// The name
		/// </summary>
		public readonly string Name;
		/// <summary>
		/// The stream. Do not dispose the stream this will be handled by the content manager/loader
		/// </summary>
		public readonly Stream Stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="NamedStream"/> structure.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <param name="stream">The stream.</param>
		public NamedStream(string name, Stream stream)
		{
			Name = name;
			Stream = stream;
		}
	}
}
