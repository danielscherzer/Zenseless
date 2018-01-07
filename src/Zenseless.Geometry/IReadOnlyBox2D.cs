using System;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Represents a read-only interface to an 2D axis aligned bounding box. 
	/// </summary>
	public interface IReadOnlyBox2D : IEquatable<IReadOnlyBox2D>
	{
		/// <summary>
		/// Size of the box in x-direction.
		/// </summary>
		float CenterX { get; }
		/// <summary>
		/// Size of the box in x-direction.
		/// </summary>
		float CenterY { get; }
		/// <summary>
		/// Maximal x-coordinate.
		/// </summary>
		float MaxX { get; }
		/// <summary>
		/// Maximal y-coordinate.
		/// </summary>
		float MaxY { get; }
		/// <summary>
		/// Minimal x-coordinate.
		/// </summary>
		float MinX { get; }
		/// <summary>
		/// Minimal y-coordinate.
		/// </summary>
		float MinY { get; }
		/// <summary>
		/// Size of the box in x-direction.
		/// </summary>
		float SizeX { get; }
		/// <summary>
		/// Size of the box in y-direction.
		/// </summary>
		float SizeY { get; }

		/// <summary>
		/// Checks if given input rectangle is inside this (including borders)
		/// </summary>
		/// <param name="rectangle">input rectangle, will be tested if it is contained inside this</param>
		/// <returns>true if given rectangle is inside this (including borders)</returns>

		bool Contains(IReadOnlyBox2D rectangle);
		/// <summary>
		/// Checks if point is inside the rectangle (including borders)
		/// </summary>
		/// <param name="x">x-coordinate of point</param>
		/// <param name="y">y-coordinate of point</param>
		/// <returns>true if point is inside the rectangle (including borders)</returns>
		bool Contains(float x, float y);

		/// <summary>
		/// Test for intersection of two rectangles (excluding borders)
		/// </summary>
		/// <param name="rectangle">second rectangle</param>
		/// <returns>true if the two rectangles overlap</returns>
		bool Intersects(IReadOnlyBox2D rectangle);

		/// <summary>
		/// Returns a string of format (MinX;MinY;SizeX;SizeY)
		/// </summary>
		/// <returns>String of format (MinX;MinY;SizeX;SizeY)</returns>
		string ToString();
	}
}