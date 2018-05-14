namespace Zenseless.Geometry
{
	using System.Numerics;

	/// <summary>
	/// A single axis rotation class that allows incremental changes
	/// </summary>
	/// <seealso cref="ITransformation" />
	public class Rotation : NotifyPropertyChanged, ITransformation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Rotation"/> class.
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="degrees">The degrees.</param>
		public Rotation(float degrees, Axis axis = Axis.Z)
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
				Matrix = MathHelper.CreateRotation(_degrees, Axis);
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix { get; private set; }

		private float _degrees;
	}
}
