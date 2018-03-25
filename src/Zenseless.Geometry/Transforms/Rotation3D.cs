using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// An single axis rotation class
	/// </summary>
	/// <seealso cref="Transformation3D" />
	public partial class Rotation3D : Transformation3D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Rotation3D"/> class.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="degrees">The degrees.</param>
		/// <param name="parent">The parent transformation</param>
		public Rotation3D(Axis axis, float degrees, Transformation3D parent = null): base(parent)
		{
			Axis = axis;
			Degrees = degrees;
		}

		/// <summary>
		/// Gets the rotation axis.
		/// </summary>
		/// <value>
		/// The rotation axis.
		/// </value>
		public Axis Axis { get; }

		/// <summary>
		/// Gets or sets the rotation degrees.
		/// </summary>
		/// <value>
		/// The rotation degrees.
		/// </value>
		public float Degrees
		{
			get
			{
				return _degrees;
			}
			set
			{
				_degrees = value;
				UpdateMatrix(MathHelper.DegreesToRadians(_degrees));
			}
		}

		private float _degrees;

		private void UpdateMatrix(float radians)
		{
			switch(Axis)
			{
				case Axis.X: matrix = Matrix4x4.CreateRotationX(radians); break;
				case Axis.Y: matrix = Matrix4x4.CreateRotationY(radians); break;
				case Axis.Z: matrix = Matrix4x4.CreateRotationZ(radians); break;
			}
		}
	}
}
