using System.Collections.Generic;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Interface for a content manager
	/// </summary>
	public interface IContentManager
	{
		/// <summary>
		/// Creates an instance of a given type from the resource with the specified name.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="name">The name of the resource to load from.</param>
		/// <returns>An instance of the given type.</returns>
		TYPE Load<TYPE>(string name) where TYPE : class;

		/// <summary>
		/// Creates an instance of a given type from the resources with the specified names.
		/// </summary>
		/// <typeparam name="TYPE">The type to create.</typeparam>
		/// <param name="names">A list of resource names.</param>
		/// <returns>An instance of the given type.</returns>
		TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class;
	}
}