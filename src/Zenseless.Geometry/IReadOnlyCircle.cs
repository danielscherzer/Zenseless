using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public interface IReadOnlyCircle
	{
		/// <summary>
		/// Gets the center.
		/// </summary>
		/// <value>
		/// The center.
		/// </value>
		Vector2 Center { get; }
		/// <summary>
		/// Gets or sets the center x.
		/// </summary>
		/// <value>
		/// The center x.
		/// </value>
		float CenterX { get; }
		/// <summary>
		/// Gets or sets the center y.
		/// </summary>
		/// <value>
		/// The center y.
		/// </value>
		float CenterY { get; }
		/// <summary>
		/// Gets or sets the radius.
		/// </summary>
		/// <value>
		/// The radius.
		/// </value>
		float Radius { get; }

		/// <summary>
		/// Determines whether the circle contains the specified point.
		/// </summary>
		/// <param name="point">The point to test.</param>
		/// <returns>
		///   <c>true</c> if the circle contains the specified point; otherwise, <c>false</c>.
		/// </returns>
		bool Contains(Vector2 point);
		
		/// <summary>
		/// Intersects the specified circle.
		/// </summary>
		/// <param name="circle">The circle.</param>
		/// <returns></returns>
		bool Intersects(IReadOnlyCircle circle);
	}
}