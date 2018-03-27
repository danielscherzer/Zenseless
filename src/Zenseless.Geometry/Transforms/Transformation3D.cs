using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Transformation class that abstracts from matrices.
	/// It can return transformation matrices in row-major or column-major style.
	/// Internally it works with the row-major matrices (<seealso cref="Matrix4x4"/>).
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
		/// Gets or sets the parent transformation.
		/// </summary>
		/// <value>
		/// The parent transformation.
		/// </value>
		public Transformation3D Parent { get; set; }

		/// <summary>
		/// Gets the local transformation matrix in column-major form.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 GetLocalColumnMajorMatrix() => Matrix4x4.Transpose(matrix);

		/// <summary>
		/// Gets the local transformation matrix in row-major form.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 GetLocalRowMajorMatrix() => matrix;

		/// <summary>
		/// Gets a local to world transformation matrix in column-major form. 
		/// This includes the whole transformation chain with all parents
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcLocalToWorldColumnMajorMatrix()
		{
			return Matrix4x4.Transpose(CalcLocalToWorldRowMajorMatrix());
		}

		/// <summary>
		/// Gets a local to world transformation matrix in row-major form. 
		/// This includes the whole transformation chain with all parents
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcLocalToWorldRowMajorMatrix()
		{
			if (Parent is null) return GetLocalRowMajorMatrix();
			return matrix * Parent.CalcLocalToWorldRowMajorMatrix();
		}

		/// <summary>
		/// The matrix field for descendants
		/// </summary>
		protected Matrix4x4 matrix = Matrix4x4.Identity;
	}
}
