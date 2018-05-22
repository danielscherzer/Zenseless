using System.Numerics;
using Zenseless.Patterns;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Implements a translation transformation that allows incremental changes
	/// </summary>
	/// <seealso cref="ITransformation" />
	public class Translation : NotifyPropertyChanged, INotifyingTransform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Translation"/> class.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <param name="z">The z-coordinate.</param>
		public Translation(float x, float y, float z)
		{
			matrix = Matrix4x4.CreateTranslation(x, y, z);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Translation"/> class.
		/// </summary>
		/// <param name="translation">The translation vector.</param>
		public Translation(Vector3 translation)
		{
			matrix = Matrix4x4.CreateTranslation(translation);
		}

		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix { get => matrix; }

		/// <summary>
		/// Gets or sets the x-coordinate.
		/// </summary>
		/// <value>
		/// The x-coordinate.
		/// </value>
		public float Tx { get { return Matrix.M41; } set { matrix.M41 = value; RaisePropertyChanged(); } }

		/// <summary>
		/// Gets or sets the y-coordinate.
		/// </summary>
		/// <value>
		/// The y-coordinate.
		/// </value>
		public float Ty { get { return Matrix.M42; } set { matrix.M42 = value; RaisePropertyChanged(); } }

		/// <summary>
		/// Gets or sets the z-coordinate.
		/// </summary>
		/// <value>
		/// The z-coordinate.
		/// </value>
		public float Tz { get { return Matrix.M43; } set { matrix.M43 = value; RaisePropertyChanged(); } }

		/// <summary>
		/// Gets or sets the translation vector.
		/// </summary>
		/// <value>
		/// The translation vector.
		/// </value>
		public Vector3 Vector { get { return Matrix.Translation; } set { matrix.Translation = value; RaisePropertyChanged(); } }

		private Matrix4x4 matrix;
	}
}
