using System.Collections.Generic;

namespace Zenseless.Geometry
{
	/// <summary>
	/// A Mesh is a collection of attributes, like positions, normals and texture coordinates
	/// </summary>
	public interface IReadOnlyMesh
	{
		/// <summary>
		/// Gets the list of attribute names.
		/// </summary>
		/// <value>
		/// The attribute names.
		/// </value>
		IEnumerable<string> AttributeNames { get; }

		/// <summary>
		/// Gets the list of ids.
		/// </summary>
		/// <value>
		/// The ids.
		/// </value>
		IEnumerable<uint> IDs { get; }

		/// <summary>
		/// Determines whether the mesh contains the attribute with the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		///   <c>true</c> if the mesh contains the attribute with the specified name; otherwise, <c>false</c>.
		/// </returns>
		bool Contains(string name);

		/// <summary>
		/// Gets the attribute with the specified name.
		/// </summary>
		/// <typeparam name="ELEMENT_TYPE">The type of the element.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		IEnumerable<ELEMENT_TYPE> Get<ELEMENT_TYPE>(string name);
	}
}