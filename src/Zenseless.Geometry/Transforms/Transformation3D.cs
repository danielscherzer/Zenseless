using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Transformation class that is based on row-major matrices
	/// </summary>
	public class Transformation3D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Transformation3D"/> class.
		/// </summary>
		/// <param name="parent">The parent transformation.</param>
		public Transformation3D(Transformation3D parent)
		{
			Parent = parent;
		}

		/// <summary>
		/// Gets or sets the parent.
		/// </summary>
		/// <value>
		/// The parent.
		/// </value>
		public Transformation3D Parent { get; set; }

		/// <summary>
		/// Gets the local transformation matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 GetLocalMatrix() => matrix;

		/// <summary>
		/// Gets the local to world transformation matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 GetLocalToWorld()
		{
			if (Parent is null) return GetLocalMatrix();
			return Parent.GetLocalToWorld() * matrix;
		}

		/// <summary>
		/// The matrix
		/// </summary>
		protected Matrix4x4 matrix = Matrix4x4.Identity;
	}
}
