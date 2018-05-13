namespace Zenseless.Geometry
{
	using System.Numerics;

	/// <summary>
	/// Interface for a transformation
	/// </summary>
	public interface ITransformation
	{
		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		Matrix4x4 Matrix { get; }
	}
}