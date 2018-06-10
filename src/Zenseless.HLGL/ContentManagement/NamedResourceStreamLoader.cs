namespace Zenseless.HLGL
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="INamedStreamLoader" />
	public class NamedResourceStreamLoader : INamedStreamLoader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NamedResourceStreamLoader"/> class.
		/// </summary>
		/// <param name="resourceAssembly">The resource assembly.</param>
		public NamedResourceStreamLoader(Assembly resourceAssembly)
		{
			ResourceAssembly = resourceAssembly;
			Names = new HashSet<string>(resourceAssembly.GetManifestResourceNames());
		}

		/// <summary>
		/// Gets the resource assembly.
		/// </summary>
		/// <value>
		/// The resource assembly.
		/// </value>
		public Assembly ResourceAssembly { get; }

		/// <summary>
		/// Gets the resource names.
		/// </summary>
		/// <value>
		/// The resource names.
		/// </value>
		public HashSet<string> Names { get; }

		IEnumerable<string> INamedStreamLoader.Names => Names;

		/// <summary>
		/// Opens a stream with the given name.
		/// </summary>
		/// <param name="name">The name of the stream.</param>
		/// <returns>
		/// A <seealso cref="NamedStream" />.
		/// </returns>
		/// <exception cref="NotImplementedException"></exception>
		public NamedStream Open(string name)
		{
			var stream = ResourceAssembly.GetManifestResourceStream(name);
			return new NamedStream(name, stream);
		}

		/// <summary>
		/// Check if the specified resource exists.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public bool Exists(string name)
		{
			return Names.Contains(name);
		}
	}
}
