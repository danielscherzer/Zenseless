namespace Zenseless.Geometry
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// A Mesh is a collection of attributes, like positions, normals and texture coordinates
	/// </summary>
	public class Mesh : IReadOnlyMesh
	{
		/// <summary>
		/// Gets the list of attribute names.
		/// </summary>
		/// <value>
		/// The attribute names.
		/// </value>
		public IEnumerable<string> AttributeNames => attributes.Keys;

		/// <summary>
		/// Gets the list of ids.
		/// </summary>
		/// <value>
		/// The ids.
		/// </value>
		public List<uint> IDs { get; private set; } = new List<uint>();

		IEnumerable<uint> IReadOnlyMesh.IDs => IDs;

		/// <summary>
		/// Adds the attribute.
		/// </summary>
		/// <param name="name">The attribute name.</param>
		/// <param name="attribute"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		internal void AddAttribute(string name, MeshAttribute attribute)
		{
			if (Contains(name)) throw new ArgumentException($"Attribute '{name}' already exists");
			if (attribute is null) throw new ArgumentNullException(nameof(attribute));
			attributes.Add(name, attribute);
		}

		/// <summary>
		/// Determines whether the mesh contains the attribute with the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		///   <c>true</c> if the mesh contains the attribute with the specified name; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string name) => attributes.ContainsKey(name);

		/// <summary>
		/// Gets the attribute with the specified name.
		/// </summary>
		/// <param name="name">The attribute name.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public MeshAttribute GetAttribute(string name)
		{
			if (attributes.TryGetValue(name, out MeshAttribute attribute))
			{
				return attribute;
			}
			throw new ArgumentException($"No attribute with name '{name}' stored.");
		}

		IEnumerable<ELEMENT_TYPE> IReadOnlyMesh.Get<ELEMENT_TYPE>(string name) => GetAttribute(name).GetList<ELEMENT_TYPE>();
		
		private Dictionary<string, MeshAttribute> attributes = new Dictionary<string, MeshAttribute>();
	}
}
