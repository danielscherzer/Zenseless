using System.Collections.Generic;
using System.IO;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public class Obj2Mesh
	{
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="IEqualityComparer{Vertex}" />
		private class VertexComparer : IEqualityComparer<ObjParser.Vertex>
		{
			/// <summary>
			/// Test if a and b are equal.
			/// </summary>
			/// <param name="a">a.</param>
			/// <param name="b">The b.</param>
			/// <returns></returns>
			public bool Equals(ObjParser.Vertex a, ObjParser.Vertex b)
			{
				return (a.idNormal == b.idNormal) && (a.idPos == b.idPos) && (a.idTexCoord == b.idTexCoord);
			}

			/// <summary>
			/// Returns a hash code for this instance.
			/// </summary>
			/// <param name="obj">The object.</param>
			/// <returns>
			/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
			/// </returns>
			public int GetHashCode(ObjParser.Vertex obj)
			{
				return obj.idPos;
			}
		}

		/// <summary>
		/// Creates a <see cref="DefaultMesh"/>from a byte array.
		/// </summary>
		/// <param name="objByteData">The byte data.</param>
		/// <returns></returns>
		public static DefaultMesh FromObj(byte[] objByteData)
		{
			var parser = new ObjParser(new MemoryStream(objByteData));
			var uniqueVertexIDs = new Dictionary<ObjParser.Vertex, uint>(new VertexComparer());

			var mesh = new DefaultMesh();

			foreach (var face in parser.faces)
			{
				//only accept triangles
				if (3 != face.Count) continue;
				foreach (var vertex in face)
				{
					if (uniqueVertexIDs.TryGetValue(vertex, out uint index))
					{
						mesh.IDs.Add(index);
					}
					else
					{
						uint id = (uint)mesh.Position.Count;
						//add vertex data to mesh
						mesh.Position.Add(parser.position[vertex.idPos]);
						if (-1 != vertex.idNormal) mesh.Normal.Add(parser.normals[vertex.idNormal]);
						if (-1 != vertex.idTexCoord) mesh.TexCoord.Add(parser.texCoords[vertex.idTexCoord]);
						mesh.IDs.Add(id);
						//new id
						uniqueVertexIDs[vertex] = id;
					}
				}
			}
			return mesh;
		}
	}
}
