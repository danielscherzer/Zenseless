using System.Numerics;
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
		public static Matrix4 Convert(this Matrix4x4 matrix)
		{
			return new Matrix4(matrix.M11, matrix.M12, matrix.M13, matrix.M14,
					matrix.M21, matrix.M22, matrix.M23, matrix.M24,
					matrix.M31, matrix.M32, matrix.M33, matrix.M34,
					matrix.M41, matrix.M42, matrix.M43, matrix.M44);
		}
	}
}
