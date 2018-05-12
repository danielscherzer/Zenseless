using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Implements a general 4x4 matrix transformation
	/// </summary>
	/// <seealso cref="Transformation3D" />
	public class MatrixTransform : Transformation3D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MatrixTransform"/> class.
		/// </summary>
		/// <param name="matrix">The transformation matrix.</param>
		/// <param name="parent">The parent.</param>
		public MatrixTransform(Matrix4x4 matrix, Transformation3D parent = null) : base(parent)
		{
			this.matrix = matrix;
		}

		/// <summary>
		/// Gets or sets the transformation matrix.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix { get => matrix; set => matrix = value; }
	}
}
