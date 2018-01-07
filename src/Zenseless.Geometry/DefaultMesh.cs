using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Geometry.Mesh" />
	public class DefaultMesh : Mesh
	{
		/// <summary>
		/// The position name
		/// </summary>
		public static readonly string PositionName = "position";
		/// <summary>
		/// The normal name
		/// </summary>
		public static readonly string NormalName = "normal";
		/// <summary>
		/// The tex coord name
		/// </summary>
		public static readonly string TexCoordName = "uv";

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public List<Vector3> Position => position;
		/// <summary>
		/// Gets the normal.
		/// </summary>
		/// <value>
		/// The normal.
		/// </value>
		public List<Vector3> Normal => normal;
		/// <summary>
		/// Gets the tex coord.
		/// </summary>
		/// <value>
		/// The tex coord.
		/// </value>
		public List<Vector2> TexCoord => texCoord;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultMesh"/> class.
		/// </summary>
		public DefaultMesh()
		{
			position = AddAttribute<Vector3>(PositionName);
			normal = AddAttribute<Vector3>(NormalName);
			texCoord = AddAttribute<Vector2>(TexCoordName);
		}

		/// <summary>
		/// The position
		/// </summary>
		private List<Vector3> position;
		/// <summary>
		/// The normal
		/// </summary>
		private List<Vector3> normal;
		/// <summary>
		/// The tex coord
		/// </summary>
		private List<Vector2> texCoord;
	}
}
