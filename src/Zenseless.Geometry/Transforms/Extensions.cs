namespace Zenseless.Geometry
{
	using System.Numerics;

	/// <summary>
	/// Extension methods
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Transforms the specified vector.
		/// </summary>
		/// <param name="vector">The vector.</param>
		/// <param name="transform">The transform.</param>
		/// <returns></returns>
		public static Vector3 Transform(this Vector3 vector, Transformation transform)
		{
			return Vector3.Transform(vector, transform.Matrix);
		}
	}
}
