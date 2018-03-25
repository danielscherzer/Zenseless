using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// An orbiting camera class
	/// </summary>
	public class CameraOrbit : ICamera
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CameraOrbit" /> class.
		/// </summary>
		public CameraOrbit()
		{
			Aspect = 1;
			Distance = 1;
			FarClip = 1;
			FovY = 90;
			Azimuth = 0;
			NearClip = 0.1f;
			Target = Vector3.Zero;
			Elevation = 0;
		}

		/// <summary>
		/// Gets or sets the aspect.
		/// </summary>
		/// <value>
		/// The aspect.
		/// </value>
		public float Aspect { get; set; }
		/// <summary>
		/// Gets or sets the azimuth.
		/// </summary>
		/// <value>
		/// The azimuth.
		/// </value>
		public float Azimuth { get; set; }
		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>
		/// The distance.
		/// </value>
		public float Distance { get; set; }
		/// <summary>
		/// Gets or sets the elevation.
		/// </summary>
		/// <value>
		/// The elevation.
		/// </value>
		public float Elevation { get; set; }
		/// <summary>
		/// Gets or sets the far clip.
		/// </summary>
		/// <value>
		/// The far clip.
		/// </value>
		public float FarClip { get; set; }
		/// <summary>
		/// Gets or sets the fov y.
		/// </summary>
		/// <value>
		/// The fov y.
		/// </value>
		public float FovY { get { return fovY; } set { fovY = MathHelper.Clamp(value, 0f, 179.9f); } }
		/// <summary>
		/// Gets or sets the near clip.
		/// </summary>
		/// <value>
		/// The near clip.
		/// </value>
		public float NearClip { get; set; }
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		public Vector3 Target { get { return target; } set { target = value; } }
		/// <summary>
		/// Gets or sets the target x.
		/// </summary>
		/// <value>
		/// The target x.
		/// </value>
		public float TargetX { get { return Target.X; } set { target.X = value; } }
		/// <summary>
		/// Gets or sets the target y.
		/// </summary>
		/// <value>
		/// The target y.
		/// </value>
		public float TargetY { get { return Target.Y; } set { target.Y = value; } }
		/// <summary>
		/// Gets or sets the target z.
		/// </summary>
		/// <value>
		/// The target z.
		/// </value>
		public float TargetZ { get { return Target.Z; } set { target.Z = value; } }

		/// <summary>
		/// Calculates the view matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcViewMatrix()
		{
			Distance = MathHelper.Clamp(Distance, NearClip, FarClip);
			var mtxDistance = Matrix4x4.Transpose(Matrix4x4.CreateTranslation(0, 0, -Distance));
			var mtxElevation = Matrix4x4.Transpose(Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(Elevation)));
			var mtxAzimut = Matrix4x4.Transpose(Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(Azimuth)));
			var mtxTarget = Matrix4x4.Transpose(Matrix4x4.CreateTranslation(-Target));
			return mtxDistance * mtxElevation * mtxAzimut * mtxTarget;
		}

		/// <summary>
		/// Calculates the projection matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcProjectionMatrix()
		{
			return Matrix4x4.Transpose(Matrix4x4.CreatePerspectiveFieldOfView(
				MathHelper.DegreesToRadians(FovY),
				Aspect, NearClip, FarClip));
		}

		/// <summary>
		/// Calculates the matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcMatrix()
		{
			return CalcProjectionMatrix() * CalcViewMatrix();
		}

		/// <summary>
		/// Calculates the position.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArithmeticException">Could not invert matrix</exception>
		public Vector3 CalcPosition()
		{
			var view = CalcViewMatrix();
			if (!Matrix4x4.Invert(view, out Matrix4x4 inverse)) throw new ArithmeticException("Could not invert matrix");
			return new Vector3(inverse.M14, inverse.M24, inverse.M34);
		}

		private float fovY;
		private Vector3 target;
	}
}
