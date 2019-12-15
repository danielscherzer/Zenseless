using OpenTK;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Conversion to OpenTK data types
	/// </summary>
	public static class Converter
	{
		/// <summary>Converts the specified matrix.</summary>
		/// <param name="matrix">The matrix to convert.</param>
		/// <returns></returns>
		public static Matrix4 Convert(this System.Numerics.Matrix4x4 matrix)
		{
			return new Matrix4(matrix.M11, matrix.M12, matrix.M13, matrix.M14,
					matrix.M21, matrix.M22, matrix.M23, matrix.M24,
					matrix.M31, matrix.M32, matrix.M33, matrix.M34,
					matrix.M41, matrix.M42, matrix.M43, matrix.M44);
		}

		/// <summary>Converts the specified vector.</summary>
		/// <param name="vector">The vector.</param>
		/// <returns></returns>
		public static Vector2 Convert(this System.Numerics.Vector2 vector) => new Vector2(vector.X, vector.Y);

		/// <summary>Converts the specified vector.</summary>
		/// <param name="vector">The vector.</param>
		/// <returns></returns>
		public static Vector3 Convert(this System.Numerics.Vector3 vector) => new Vector3(vector.X, vector.Y, vector.Z);

		/// <summary>Converts the specified vector.</summary>
		/// <param name="vector">The vector.</param>
		/// <returns></returns>
		public static Vector4 Convert(this System.Numerics.Vector4 vector) => new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}
}
