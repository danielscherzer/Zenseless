using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// static class of <seealso cref="IReadOnlyCircle"/> extension methods
	/// </summary>
	public static class IReadOnlyCircleExtensions
	{
		/// <summary>
		/// Determines whether the circle contains the specified point.
		/// </summary>
		/// <param name="circle">The circle to test.</param>
		/// <param name="point">The point to test.</param>
		/// <returns>
		///   <c>true</c> if the circle contains the specified point; otherwise, <c>false</c>.
		/// </returns>
		public static bool Contains(this IReadOnlyCircle circle, Vector2 point)
		{
			var diff = point - circle.Center;
			return circle.Radius * circle.Radius > diff.LengthSquared();
		}

		/// <summary>
		/// Test the specified circles for intersection.
		/// </summary>
		/// <param name="circleA">The first circle.</param>
		/// <param name="circleB">The second circle.</param>
		/// <returns><c>true</c> if the circles overlap; otherwise, <c>false</c></returns>
		public static bool Intersects(this IReadOnlyCircle circleA, IReadOnlyCircle circleB)
		{
			var rSum = circleB.Radius + circleA.Radius;
			var diff = circleB.Center - circleA.Center;
			var ll = diff.LengthSquared();
			return rSum * rSum > diff.LengthSquared();
		}
	}
}
