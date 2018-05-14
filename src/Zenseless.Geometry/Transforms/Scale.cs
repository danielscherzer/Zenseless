using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Implements a scale transformation that allows incremental changes
	/// </summary>
	/// <seealso cref="ITransformation" />
	public class Scale : NotifyPropertyChanged, ITransformation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Scale"/> class.
		/// </summary>
		/// <param name="uniformScale">The uniform scale factor.</param>
		public Scale(float uniformScale)
		{
			matrix = Matrix4x4.CreateScale(uniformScale);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Scale"/> class.
		/// </summary>
		/// <param name="sx">The x.</param>
		/// <param name="sy">The y.</param>
		/// <param name="sz">The z.</param>
		public Scale(float sx, float sy, float sz)
		{
			matrix = Matrix4x4.CreateScale(sx, sy, sz);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Scale"/> class.
		/// </summary>
		/// <param name="scaleVector">The scale vector.</param>
		public Scale(Vector3 scaleVector)
		{
			matrix = Matrix4x4.CreateScale(scaleVector);
		}

		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix => matrix;

		/// <summary>
		/// Gets or sets the x-coordinate.
		/// </summary>
		/// <value>
		/// The x-coordinate.
		/// </value>
		public float Sx { get { return matrix.M11; } set { matrix.M11 = value; RaisePropertyChanged(); } }

		/// <summary>
		/// Gets or sets the y-coordinate.
		/// </summary>
		/// <value>
		/// The y-coordinate.
		/// </value>
		public float Sy { get { return matrix.M22; } set { matrix.M22 = value; RaisePropertyChanged(); } }

		/// <summary>
		/// Gets or sets the z-coordinate.
		/// </summary>
		/// <value>
		/// The z-coordinate.
		/// </value>
		public float Sz { get { return matrix.M33; } set { matrix.M33 = value; RaisePropertyChanged(); } }

		private Matrix4x4 matrix;
	}
}
