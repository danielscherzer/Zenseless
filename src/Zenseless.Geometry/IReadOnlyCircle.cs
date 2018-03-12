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
	}
}