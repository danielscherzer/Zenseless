using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// A content loader interface
	/// </summary>
	public interface IContentLoader
	{
		/// <summary>
		/// Enumerates all content resource names.
		/// </summary>
		/// <value>
		/// All content resource names.
		/// </value>
		IEnumerable<string> Names { get; }

		/// <summary>
		/// Creates an instance of a given type from the resources with the specified names.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="names">A list of resource names.</param>
		/// <returns>An instance of the given type.</returns>
		TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class;
	}
}