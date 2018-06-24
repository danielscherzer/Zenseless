namespace Zenseless.Geometry
{
	using System;
	using System.Numerics;
	using Zenseless.Patterns;

	/// <summary>
	/// Implements a orbiting transformation
	/// </summary>
	/// <seealso cref="ITransformation" />
	public class Orbit : NotifyPropertyChanged, INotifyingTransform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Orbit"/> class.
		/// </summary>
		/// <param name="distance">The distance to the target.</param>
		/// <param name="azimuth">The azimuth or heading.</param>
		/// <param name="elevation">The elevation or tilt.</param>
		public Orbit(float distance = 1f, float azimuth = 0f, float elevation = 0f)
		{
			cachedMatrixView = new CachedCalculatedValue<Matrix4x4>(CalcViewMatrix);
			PropertyChanged += (s, a) => cachedMatrixView.Invalidate();
			Azimuth = azimuth;
			Distance = distance;
			Elevation = elevation;

		}

		/// <summary>
		/// Gets or sets the azimuth or heading.
		/// </summary>
		/// <value>
		/// The azimuth.
		/// </value>
		public float Azimuth
		{
			get => _azimuth;
			set
			{
				_azimuth = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the distance from the target.
		/// </summary>
		/// <value>
		/// The distance.
		/// </value>
		public float Distance
		{
			get => _distance;
			set
			{
				_distance = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the elevation or tilt.
		/// </summary>
		/// <value>
		/// The elevation.
		/// </value>
		public float Elevation
		{
			get => _elevation;
			set
			{
				_elevation = value;
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
		/// Gets or sets the target, the point the camera is looking at.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		public Vector3 Target
		{
			get => _target;
			set
			{
				_target = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the target x.
		/// </summary>
		/// <value>
		/// The target x.
		/// </value>
		public float TargetX
		{
			get => Target.X;
			set
			{
				_target.X = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the target y.
		/// </summary>
		/// <value>
		/// The target y.
		/// </value>
		public float TargetY
		{
			get => Target.Y;
			set
			{
				_target.Y = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the target z.
		/// </summary>
		/// <value>
		/// The target z.
		/// </value>
		public float TargetZ
		{
			get => Target.Z;
			set
			{
				_target.Z = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Calculates the camera position.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArithmeticException">Could not invert matrix</exception>
		public Vector3 CalcPosition()
		{
			if (!Matrix4x4.Invert(Matrix, out Matrix4x4 inverse)) throw new ArithmeticException("Could not invert matrix");
			return inverse.Translation;
		}

		private float _azimuth = 0f;
		private float _distance = 0f;
		private float _elevation = 0f;
		private Vector3 _target = Vector3.Zero;
		private CachedCalculatedValue<Matrix4x4> cachedMatrixView;

		private Matrix4x4 CalcViewMatrix()
		{
			var mtxDistance = Matrix4x4.CreateTranslation(0, 0, -Distance);
			var mtxElevation = Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(Elevation));
			var mtxAzimut = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(Azimuth));
			var mtxTarget = Matrix4x4.CreateTranslation(-Target);
			return mtxTarget * mtxAzimut * mtxElevation * mtxDistance;
		}
	}
}
