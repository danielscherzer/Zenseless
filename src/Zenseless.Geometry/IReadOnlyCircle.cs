using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// A read only interface to a circle
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
		/// Gets the center x-coordinate.
		/// </summary>
		/// <value>
		/// The center x-coordinate.
		/// </value>
		float CenterX { get; }
		
		/// <summary>
		/// Gets the center y-coordinate.
		/// </summary>
		/// <value>
		/// The center y-coordinate.
		/// </value>
		float CenterY { get; }
		
		/// <summary>
		/// Gets the radius.
		/// </summary>
		/// <value>
		/// The radius.
		/// </value>
		float Radius { get; }
	}
}