using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Transformation class that is based on row-major matrices
	/// </summary>
	public class Transformation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Transformation"/> class.
		/// </summary>
		public Transformation()
		{
			Reset();
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="Transformation"/> to <see cref="Matrix4x4"/>.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator Matrix4x4(Transformation t)
		{
			return t.matrix;
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		public void Reset()
		{
			matrix = Matrix4x4.Identity;
		}

		/// <summary>
		/// Rotate Transform
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateXGlobal(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Rotates the y global.
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateYGlobal(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Rotates the z global.
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateZGlobal(float degrees)
		{
			TransformGlobal(Matrix4x4.CreateRotationZ(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Rotates the x local.
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateXLocal(float degrees)
		{
			TransformLocal(Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Rotates the y local.
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateYLocal(float degrees)
		{
			TransformLocal(Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Rotates the z local.
		/// </summary>
		/// <param name="degrees">The degrees.</param>
		public void RotateZLocal(float degrees)
		{
			TransformLocal(Matrix4x4.CreateRotationZ(MathHelper.DegreesToRadians(degrees)));
		}

		/// <summary>
		/// Scales the global.
		/// </summary>
		/// <param name="scales">The scales.</param>
		public void ScaleGlobal(Vector3 scales)
		{
			TransformGlobal(Matrix4x4.CreateScale(scales));
		}

		/// <summary>
		/// Scales the global.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		public void ScaleGlobal(float x, float y, float z)
		{
			TransformGlobal(Matrix4x4.CreateScale(x, y, z));
		}

		/// <summary>
		/// Scales the local.
		/// </summary>
		/// <param name="scales">The scales.</param>
		public void ScaleLocal(Vector3 scales)
		{
			TransformLocal(Matrix4x4.CreateScale(scales));
		}

		/// <summary>
		/// Scales the local.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		public void ScaleLocal(float x, float y, float z)
		{
			TransformLocal(Matrix4x4.CreateScale(x, y, z));
		}

		/// <summary>
		/// Translates the global.
		/// </summary>
		/// <param name="translation">The translation.</param>
		public void TranslateGlobal(Vector3 translation)
		{
			TransformGlobal(Matrix4x4.CreateTranslation(translation));
		}

		/// <summary>
		/// Translates the global.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		public void TranslateGlobal(float x, float y, float z)
		{
			TransformGlobal(Matrix4x4.CreateTranslation(x, y, z));
		}

		/// <summary>
		/// Translates the local.
		/// </summary>
		/// <param name="translation">The translation.</param>
		public void TranslateLocal(Vector3 translation)
		{
			TransformLocal(Matrix4x4.CreateTranslation(translation));
		}

		/// <summary>
		/// Translates the local.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		public void TranslateLocal(float x, float y, float z)
		{
			TransformLocal(Matrix4x4.CreateTranslation(x, y, z));
		}

		/// <summary>
		/// Transforms the specified position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns></returns>
		public Vector3 Transform(Vector3 position)
		{
			return Vector3.Transform(position, matrix);
		}

		/// <summary>
		/// Transforms the global.
		/// </summary>
		/// <param name="transform">The transform.</param>
		public void TransformGlobal(Matrix4x4 transform)
		{
			matrix *= transform;
		}

		/// <summary>
		/// Transforms the local.
		/// </summary>
		/// <param name="transform">The transform.</param>
		public void TransformLocal(Matrix4x4 transform)
		{
			matrix = transform * matrix;
		}

		/// <summary>
		/// The matrix
		/// </summary>
		private Matrix4x4 matrix;
	}
}
