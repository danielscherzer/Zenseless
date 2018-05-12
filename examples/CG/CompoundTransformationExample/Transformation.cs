namespace Zenseless.Geometry
{
	using System;
	using System.Numerics;

	/// <summary>
	/// Transformation class that abstracts from matrices.
	/// It can return transformation matrices in row-major or column-major style.
	/// Internally it works with the row-major matrices (<seealso cref="Matrix4x4"/>).
	/// </summary>
	public struct Transformation
	{
		public Transformation(in Matrix4x4 matrix)
		{
			this.matrix = matrix;
		}

		/// <summary>
		/// Gets the local transformation matrix in column-major form.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 GetLocalColumnMajorMatrix() => Matrix4x4.Transpose(matrix);

		/// <summary>
		/// Gets the local transformation matrix in row-major form.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 GetLocalRowMajorMatrix() => matrix;

		public static Transformation Combine(in Transformation transform, in Transformation parent)
		{
			return new Transformation(transform.matrix * parent.matrix);
		}

		public static Transformation Identity()
		{
			return new Transformation(Matrix4x4.Identity);
		}

		public static Transformation Translation(float x, float y, float z)
		{
			return new Transformation(Matrix4x4.CreateTranslation(x, y, z));
		}

		public static Transformation Rotation(Axis axis, float degrees)
		{
			return new Transformation(CreateRotation(axis, degrees));
		}

		private Matrix4x4 matrix;

		private static Matrix4x4 CreateRotation(Axis axis, float degrees)
		{
			var radians = MathHelper.DegreesToRadians(degrees);
			switch (axis)
			{
				case Axis.X: return Matrix4x4.CreateRotationX(radians);
				case Axis.Y: return Matrix4x4.CreateRotationY(radians);
				case Axis.Z: return Matrix4x4.CreateRotationZ(radians);
				default: throw new ArgumentException($"Unknown axis:{axis}");
			}
		}
	}
}
