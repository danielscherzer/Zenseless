using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Implements a translation transformation
	/// </summary>
	/// <seealso cref="Transformation3D" />
	public class Translation3D : Transformation3D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Translation"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <param name="parent">The parent transformation</param>
		public Translation3D(float x, float y, float z, Transformation3D parent = null) : base(parent)
		{
			matrix = Matrix4x4.CreateTranslation(x, y, z);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Translation"/> class.
		/// </summary>
		/// <param name="translation">The translation.</param>
		/// <param name="parent">The parent transformation</param>
		public Translation3D(Vector3 translation, Transformation3D parent = null) : base(parent)
		{
			matrix = Matrix4x4.CreateTranslation(translation);
		}

		/// <summary>
		/// Gets or sets the x-coordinate.
		/// </summary>
		/// <value>
		/// The x-coordinate.
		/// </value>
		public float Tx { get { return matrix.M41; } set { matrix.M41 = value; } }

		/// <summary>
		/// Gets or sets the y-coordinate.
		/// </summary>
		/// <value>
		/// The y-coordinate.
		/// </value>
		public float Ty { get { return matrix.M42; } set { matrix.M42 = value; } }

		/// <summary>
		/// Gets or sets the z-coordinate.
		/// </summary>
		/// <value>
		/// The z-coordinate.
		/// </value>
		public float Tz { get { return matrix.M43; } set { matrix.M43 = value; } }

		/// <summary>
		/// Gets or sets the translation vector.
		/// </summary>
		/// <value>
		/// The translation vector.
		/// </value>
		public Vector3 Translation { get { return matrix.Translation; } set { matrix.Translation = value; } }

	}
}
