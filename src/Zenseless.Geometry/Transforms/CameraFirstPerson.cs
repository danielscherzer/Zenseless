using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// A first person camera with position, heading, tilt
	/// </summary>
	public class CameraFirstPerson
	{
		/// <summary>
		/// Gets or sets the aspect ratio.
		/// </summary>
		/// <value>
		/// The aspect ratio.
		/// </value>
		public float Aspect { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the far clipping plane distance.
		/// </summary>
		/// <value>
		/// The far clipping plane distance.
		/// </value>
		public float FarClip { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the field-of-view y.
		/// </summary>
		/// <value>
		/// The field-of-view y.
		/// </value>
		public float FovY { get; set; } = 90f;

		/// <summary>
		/// Gets or sets the heading in degrees.
		/// </summary>
		/// <value>
		/// The heading.
		/// </value>
		public float Heading { get; set; } = 0f;

		/// <summary>
		/// Gets or sets the near clipping plane distance.
		/// </summary>
		/// <value>
		/// The near clipping plane distance.
		/// </value>
		public float NearClip { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public Vector3 Position { get; set; } = Vector3.Zero;

		/// <summary>
		/// Gets or sets the tilt in degrees.
		/// </summary>
		/// <value>
		/// The tilt.
		/// </value>
		public float Tilt { get; set; } = 0f;


		/// <summary>
		/// Calculates the complete transformation matrix (projection and view).
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcMatrix()
		{
			return CalcProjectionMatrix() * CalcViewMatrix();
		}

		/// <summary>
		/// Calculates the projection matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcProjectionMatrix()
		{
			FovY = MathHelper.Clamp(FovY, 0f, 179.9f);
			var fov = MathHelper.DegreesToRadians(FovY);
			return Matrix4x4.Transpose(Matrix4x4.CreatePerspectiveFieldOfView(fov, Aspect, NearClip, FarClip));
		}

		/// <summary>
		/// Calculates the rotation matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcRotationMatrix()
		{
			var heading = MathHelper.DegreesToRadians(Heading);
			var tilt = MathHelper.DegreesToRadians(Tilt);
			var rotX = Matrix4x4.Transpose(Matrix4x4.CreateRotationX(tilt));
			var rotY = Matrix4x4.Transpose(Matrix4x4.CreateRotationY(heading));
			var mtxRotate = rotX * rotY;
			return mtxRotate;
		}

		/// <summary>
		/// Calculates the view matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcViewMatrix()
		{
			var t = new Translation3D(-Position);
			var mtxTranslate = Matrix4x4.Transpose(Matrix4x4.CreateTranslation(-Position));
			var mtxRotate = CalcRotationMatrix();
			return mtxRotate * mtxTranslate;
		}

		public void ApplyRotatedMovement(Vector3 movement)
		{
			var rotation = CalcRotationMatrix();
			Position += Vector3.Transform(movement, rotation);
		}
	}
}
