using System;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Represents a read-only interface to an 2D axis aligned bounding box. 
	/// </summary>
	public interface IReadOnlyBox2D : IEquatable<IReadOnlyBox2D> //TODO: rename to Rectangle for next semester
	{
		/// <summary>
		/// Center of the rectangle in x-direction.
		/// </summary>
		float CenterX { get; }

		/// <summary>
		/// Center of the rectangle in x-direction.
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
	}
}