namespace Zenseless.Geometry
{
	using System.Collections.Generic;
	using System.Numerics;

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Mesh" />
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
		public List<Vector3> Position { get; }

		//public List<Vector3> Position => Get<Vector3>(PositionName);
		/// <summary>
		/// Gets the normal.
		/// </summary>
		/// <value>
		/// The normal.
		/// </value>
		public List<Vector3> Normal { get; }

		/// <summary>
		/// Gets the tex coord.
		/// </summary>
		/// <value>
		/// The tex coord.
		/// </value>
		public List<Vector2> TexCoord { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultMesh"/> class.
		/// </summary>
		public DefaultMesh()
		{
			AddAttribute(PositionName, MeshAttribute.Create<Vector3>());
			Position = GetAttribute(PositionName).GetList<Vector3>();
			AddAttribute(NormalName, MeshAttribute.Create<Vector3>());
			Normal = GetAttribute(NormalName).GetList<Vector3>();
			AddAttribute(TexCoordName, MeshAttribute.Create<Vector2>());
			TexCoord = GetAttribute(TexCoordName).GetList<Vector2>();
		}
	}
}
