using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// static class of <seealso cref="IReadOnlyBox2D"/> extension methods
	/// </summary>
	public static class IReadOnlyBox2Dextensions
	{
		/// <summary>
		/// Checks if a point is inside a rectangle (including borders)
		/// </summary>
		/// <param name="rectangle">A rectangle</param>
		/// <param name="x">The x-coordinate of point</param>
		/// <param name="y">The y-coordinate of point</param>
		/// <returns>true if point is inside the rectangle (including borders)</returns>
		public static bool Contains(this IReadOnlyBox2D rectangle, float x, float y)
		{
			if (x < rectangle.MinX || rectangle.MaxX < x) return false;
			if (y < rectangle.MinY || rectangle.MaxY < y) return false;
			return true;
		}

		/// <summary>
		/// Checks if point is inside the rectangle (including borders)
		/// </summary>
		/// <param name="rectangle">Rectangle to check</param>
		/// <param name="point">Coordinates of the point</param>
		/// <returns>true if point is inside the rectangle (including borders)</returns>
		public static bool Contains(this IReadOnlyBox2D rectangle, Vector2 point) => rectangle.Contains(point.X, point.Y);

		/// <summary>
		/// Checks if rectangle contains a second rectangle (including borders)
		/// </summary>
		/// <param name="a">container rectangle, will be tested if contains the other</param>
		/// <param name="b">rectangle, will be tested if it is contained by the other</param>
		/// <returns>true if rectangle contains a second rectangle (including borders)</returns>
		public static bool Contains(this IReadOnlyBox2D a, IReadOnlyBox2D b)
		{
			if (a.MinX > b.MinX) return false;
			if (a.MaxX < b.MaxX) return false;
			if (a.MinY > b.MinY) return false;
			if (a.MaxY < b.MaxY) return false;
			return true;
		}

		/// <summary>
		/// Gets the center of the box.
		/// </summary>
		/// <param name="box">The box.</param>
		/// <returns></returns>
		public static Vector2 GetCenter(this IReadOnlyBox2D box)
		{
			return new Vector2(box.CenterX, box.CenterY);
		}

		/// <summary>
		/// Test for intersection of two rectangles (excluding borders)
		/// </summary>
		/// <param name="a">A rectangle</param>
		/// <param name="b">A second rectangle</param>
		/// <returns>true if the two rectangles overlap</returns>
		public static bool Intersects(this IReadOnlyBox2D a, IReadOnlyBox2D b)
		{
			bool noXintersect = (a.MaxX <= b.MinX) || (a.MinX >= b.MaxX);
			bool noYintersect = (a.MaxY <= b.MinY) || (a.MinY >= b.MaxY);
			return !(noXintersect || noYintersect);
		}
	}
}
