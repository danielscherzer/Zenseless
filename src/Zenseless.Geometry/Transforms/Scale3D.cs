using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Implements a translation transformation
	/// </summary>
	/// <seealso cref="Transformation3D" />
	public class Scale3D : Transformation3D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Scale3D"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <param name="parent">The parent transformation</param>
		public Scale3D(float x, float y, float z, Transformation3D parent = null) : base(parent)
		{
			matrix = Matrix4x4.CreateScale(x, y, z);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Scale3D"/> class.
		/// </summary>
		/// <param name="scaleVector">The translation.</param>
		/// <param name="parent">The parent transformation</param>
		public Scale3D(Vector3 scaleVector, Transformation3D parent = null) : base(parent)
		{
			matrix = Matrix4x4.CreateScale(scaleVector);
		}

		/// <summary>
		/// Gets or sets the x-coordinate.
		/// </summary>
		/// <value>
		/// The x-coordinate.
		/// </value>
		public float Sx { get { return matrix.M11; } set { matrix.M11 = value; } }

		/// <summary>
		/// Gets or sets the y-coordinate.
		/// </summary>
		/// <value>
		/// The y-coordinate.
		/// </value>
		public float Sy { get { return matrix.M22; } set { matrix.M22 = value; } }

		/// <summary>
		/// Gets or sets the z-coordinate.
		/// </summary>
		/// <value>
		/// The z-coordinate.
		/// </value>
		public float Sz { get { return matrix.M33; } set { matrix.M33 = value; } }
	}
}
