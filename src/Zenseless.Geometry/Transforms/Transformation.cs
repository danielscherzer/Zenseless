namespace Zenseless.Geometry
{
	using System.Numerics;

	/// <summary>
	/// Immutable transformation structure that is used to abstract from matrices. 
	/// Do not use the parameterless default constructor because he initializes the transform not to the identity, but to the 0 transform.
	/// Internally it uses row-major matrices (<seealso cref="Matrix4x4"/>).
	/// </summary>
	public struct Transformation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Transformation" /> structure from a matrix.
		/// </summary>
		/// <param name="matrix">The matrix.</param>
		public Transformation(in Matrix4x4 matrix)
		{
			Matrix = matrix;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Transformation" /> structure.
		/// </summary>
		/// <param name="transformation">The transformation.</param>
		public Transformation(in ITransformation transformation)
		{
			Matrix = transformation.Matrix;
		}

		/// <summary>
		/// Gets the local transformation matrix in row-major form.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 Matrix { get; }

		/// <summary>
		/// Calculates the combined transformation that is equal to first applying the localTransform and afterwards the parent transform to input points.
		/// </summary>
		/// <param name="localTransform">The local transform.</param>
		/// <param name="parentTransform">The parent transform.</param>
		/// <returns>The combined transformation</returns>
		public static Transformation Combine(in Transformation localTransform, in Transformation parentTransform)
		{
			return new Transformation(localTransform.Matrix * parentTransform.Matrix);
		}

		/// <summary>
		/// Creates an identity transform.
		/// </summary>
		/// <returns></returns>
		public static Transformation Identity => new Transformation(Matrix4x4.Identity);

		/// <summary>
		/// Creates a scale transform.
		/// </summary>
		/// <param name="scale">The scale factor.</param>
		/// <returns></returns>
		public static Transformation Scale(float scale)
		{
			return new Transformation(Matrix4x4.CreateScale(scale));
		}

		/// <summary>
		/// Creates a scale transform.
		/// </summary>
		/// <param name="sx">The scale factor in x-direction.</param>
		/// <param name="sy">The scale factor in y-direction.</param>
		/// <param name="sz">The scale factor in z-direction.</param>
		/// <returns></returns>
		public static Transformation Scale(float sx, float sy, float sz = 1f)
		{
			return new Transformation(Matrix4x4.CreateScale(sx, sy, sz));
		}

		/// <summary>
		/// Creates a translation transform.
		/// </summary>
		/// <param name="tx">The translation in x-direction</param>
		/// <param name="ty">The translation in y-direction</param>
		/// <param name="tz">The translation in z-direction</param>
		/// <returns></returns>
		public static Transformation Translation(float tx, float ty, float tz = 0f)
		{
			return new Transformation(Matrix4x4.CreateTranslation(tx, ty, tz));
		}

		/// <summary>
		/// Transforms the specified vector.
		/// </summary>
		/// <param name="position">The input position.</param>
		/// <returns></returns>
		public Vector2 Transform(in Vector2 position)
		{
			return Vector2.Transform(position, Matrix);
		}

		/// <summary>
		/// Transforms the specified vector.
		/// </summary>
		/// <param name="position">The input position.</param>
		/// <returns></returns>
		public Vector3 Transform(in Vector3 position)
		{
			return Vector3.Transform(position, Matrix);
		}

		/// <summary>
		/// Transforms the specified vector.
		/// </summary>
		/// <param name="vector">The input vector.</param>
		/// <returns></returns>
		public Vector4 Transform(in Vector4 vector)
		{
			return Vector4.Transform(vector, Matrix);
		}

		/// <summary>
		/// Creates a translation transform.
		/// </summary>
		/// <param name="t">The translation vector</param>
		/// <returns></returns>
		public static Transformation Translation(in Vector3 t)
		{
			return new Transformation(Matrix4x4.CreateTranslation(t));
		}

		/// <summary>
		/// Creates a rotation transform.
		/// </summary>
		/// <param name="degrees">The rotation angle in degrees.</param>
		/// <param name="axis">The axis of rotation.</param>
		/// <returns></returns>
		public static Transformation Rotation(float degrees, Axis axis = Axis.Z)
		{
			return new Transformation(MathHelper.CreateRotation(degrees, axis));
		}

		/// <summary>
		/// Creates a rotation transform that rotates around a given rotation center (pivot point)
		/// </summary>
		/// <param name="pivot">pivot point</param>
		/// <param name="degrees">rotation in degrees</param>
		/// <returns>A <see cref="Transformation"/> that rotates around a given pivot point.</returns>
		public static Transformation RotationAround(in Vector3 pivot, float degrees)
		{
			return Combine(Translation(-pivot), Combine(Rotation(degrees), Translation(pivot)));
		}
	}
}
