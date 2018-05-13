namespace Zenseless.Geometry
{
	using System;
	using System.Numerics;

	/// <summary>
	/// Immutable transformation structure that abstracts from matrices. 
	/// Do not use the parameterless default constructor because he initializes the transform not to the identity, but to the 0 transform.
	/// Internally it works with the row-major matrices (<seealso cref="Matrix4x4"/>).
	/// </summary>
	public struct Transformation : ITransformation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Transformation" /> structure.
		/// </summary>
		/// <param name="matrix">The matrix.</param>
		public Transformation(in Matrix4x4 matrix)
		{
			Matrix = matrix;
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="Transformation" /> to <see cref="Matrix4x4" />.
		/// </summary>
		/// <param name="transformation">The transformation.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator Matrix4x4(Transformation transformation)
		{
			return transformation.Matrix;
		}

		/// <summary>
		/// Gets the local transformation matrix in row-major form.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 Matrix { get; }

		/// <summary>
		/// Combines the two specified transforms.
		/// </summary>
		/// <param name="localTransform">The local transform.</param>
		/// <param name="parent">The parent transform.</param>
		/// <returns></returns>
		public static Transformation Combine(in ITransformation localTransform, in ITransformation parent)
		{
			return new Transformation(localTransform.Matrix * parent.Matrix);
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
		public static Transformation Scale(float sx, float sy, float sz)
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
		public static Transformation Translation(float tx, float ty, float tz)
		{
			return new Transformation(Matrix4x4.CreateTranslation(tx, ty, tz));
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
	}
}
