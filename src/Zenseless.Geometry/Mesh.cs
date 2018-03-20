using System;
using System.Collections.Generic;

namespace Zenseless.Geometry
{
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
		/// <typeparam name="ELEMENT_TYPE">The type of the element.</typeparam>
		/// <param name="name">The attribute name.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public List<ELEMENT_TYPE> AddAttribute<ELEMENT_TYPE>(string name)
		{
			if (Contains(name)) throw new ArgumentException($"Attribute '{name}' already exists");
			var attribute = new List<ELEMENT_TYPE>();
			attributes.Add(name, new List<ELEMENT_TYPE>());
			return attribute;
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
		/// <typeparam name="ELEMENT_TYPE">The type of the element.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public List<ELEMENT_TYPE> Get<ELEMENT_TYPE>(string name)
		{
			if (attributes.TryGetValue(name, out object data))
			{
				var typedData = data as List<ELEMENT_TYPE>;
				if(typedData is null)
				{
					throw new InvalidCastException($"Attribute '{name}' has type {data.GetType().FullName}.");
				}
				return typedData;
			}
			throw new ArgumentException($"No attribute with name '{name}' stored.");
		}

		IEnumerable<ELEMENT_TYPE> IReadOnlyMesh.Get<ELEMENT_TYPE>(string name) => Get<ELEMENT_TYPE>(name);

		private Dictionary<string, object> attributes = new Dictionary<string, object>();
	}
}
