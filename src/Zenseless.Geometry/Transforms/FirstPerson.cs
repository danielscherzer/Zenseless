namespace Zenseless.Geometry
{
	using System.Numerics;
	using Zenseless.Patterns;

	/// <summary>
	/// Implements a orbiting transformation
	/// </summary>
	/// <seealso cref="ITransformation" />
	public class FirstPerson : NotifyPropertyChanged, INotifyingTransform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FirstPerson"/> class.
		/// </summary>
		/// <param name="position">The position of the camera.</param>
		/// <param name="heading">The heading.</param>
		/// <param name="tilt">The tilt.</param>
		public FirstPerson(Vector3 position, float heading = 0f, float tilt = 0f)
		{
			cachedMatrixView = new CachedCalculatedValue<Matrix4x4>(CalcViewMatrix);
			PropertyChanged += (s, a) => cachedMatrixView.Invalidate();
			Position = position;
			Heading = heading;
			Tilt = tilt;

		}

		/// <summary>
		/// Applies the rotated movement vector to the position of the camera.
		/// </summary>
		/// <param name="movement">The movement vector.</param>
		public void ApplyRotatedMovement(in Vector3 movement)
		{
			var rotation = Matrix4x4.Transpose(CalcRotationMatrix());
			Position += Vector3.Transform(movement, rotation);
		}

		/// <summary>
		/// Gets or sets the heading.
		/// </summary>
		public float Heading
		{
			get => _heading;
			set
			{
				_heading = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix => cachedMatrixView.Value;

		/// <summary>
		/// Gets or sets the position, the point the camera is positioned at.
		/// </summary>
		public Vector3 Position
		{
			get => _position;
			set
			{
				_position = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the tilt.
		/// </summary>
		public float Tilt
		{
			get => _tilt;
			set
			{
				_tilt = value;
				RaisePropertyChanged();
			}
		}

		private float _heading = 0f;
		private float _tilt = 0f;
		private Vector3 _position = Vector3.Zero;
		private CachedCalculatedValue<Matrix4x4> cachedMatrixView;

		private Matrix4x4 CalcRotationMatrix()
		{
			var heading = MathHelper.DegreesToRadians(Heading);
			var tilt = MathHelper.DegreesToRadians(Tilt);
			var rotX = Matrix4x4.CreateRotationX(tilt);
			var rotY = Matrix4x4.CreateRotationY(heading);
			var mtxRotate = rotY * rotX;
			return mtxRotate;
		}

		private Matrix4x4 CalcViewMatrix()
		{
			var mtxTranslate = Matrix4x4.CreateTranslation(-Position);
			var mtxRotate = CalcRotationMatrix();
			return mtxTranslate * mtxRotate;
		}
	}
}
