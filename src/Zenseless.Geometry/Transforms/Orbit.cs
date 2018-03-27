using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Implements a orbiting transformation
	/// </summary>
	/// <seealso cref="Transformation3D" />
	public class Orbit : Transformation3D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Orbit"/> class.
		/// </summary>
		/// <param name="distance">The distance to the target.</param>
		/// <param name="azimuth">The azimuth or heading.</param>
		/// <param name="elevation">The elevation or tilt.</param>
		/// <param name="parent">The parent transformation.</param>
		public Orbit(float distance = 1f, float azimuth = 0f, float elevation = 0f, Transformation3D parent = null) : base(parent)
		{
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
			get => _azimuth; set
			{
				_azimuth = value;
				UpdateMatrix();
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
			get => _distance; set
			{
				_distance = value;
				UpdateMatrix();
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
				UpdateMatrix();
			}
		}

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
				UpdateMatrix();
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
				UpdateMatrix();
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
				UpdateMatrix();
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
				UpdateMatrix();
			}
		}

		/// <summary>
		/// Calculates the camera position.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArithmeticException">Could not invert matrix</exception>
		public Vector3 CalcPosition()
		{
			if (!Matrix4x4.Invert(matrix, out Matrix4x4 inverse)) throw new ArithmeticException("Could not invert matrix");
			return inverse.Translation;
		}

		private float _azimuth = 0f;
		private float _distance = 0f;
		private float _elevation = 0f;
		private Vector3 _target = Vector3.Zero;

		private void UpdateMatrix()
		{
			var mtxDistance = Matrix4x4.CreateTranslation(0, 0, -Distance);
			var mtxElevation = Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(Elevation));
			var mtxAzimut = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(Azimuth));
			var mtxTarget = Matrix4x4.CreateTranslation(-Target);
			matrix = mtxTarget * mtxAzimut * mtxElevation * mtxDistance;
		}
	}
}
