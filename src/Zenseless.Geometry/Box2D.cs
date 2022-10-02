using System;
using System.Diagnostics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// This class represents a mutable 2D axis aligned bounding box. 
	/// It is a class, because Microsoft recommends structures to be immutable 
	/// and this class will be often used as a function parameter, so less
	/// copying is necessary.
	/// </summary>
	[Serializable]
	public class Box2D : IReadOnlyBox2D
	{
		/// <summary>
		/// Creates an 2D axis aligned bounding box
		/// </summary>
		/// <param name="minX">minimal x-coordinate</param>
		/// <param name="minY">minimal y-coordinate</param>
		/// <param name="sizeX">width</param>
		/// <param name="sizeY">height</param>
		public Box2D(float minX, float minY, float sizeX, float sizeY)
		{
			Debug.Assert(sizeX >= 0);
			Debug.Assert(sizeY >= 0);
			this.MinX = minX;
			this.MinY = minY;
			this.SizeX = sizeX;
			this.SizeY = sizeY;
		}

		/// <summary>
		/// Creates an 2D axis aligned bounding box.
		/// </summary>
		/// <param name="rectangle">Source rectangle to copy.</param>
		public Box2D(IReadOnlyBox2D rectangle)
		{
			this.MinX = rectangle.MinX;
			this.MinY = rectangle.MinY;
			this.SizeX = rectangle.SizeX;
			this.SizeY = rectangle.SizeY;
		}

		/// <summary>
		/// Box from coordinates [0,0] to [1,1].
		/// </summary>
		public static readonly IReadOnlyBox2D BOX01 = new Box2D(0, 0, 1, 1);
		/// <summary>
		/// X-coordinate of the center of the box. Setting the value will move the box, while to size will not change.
		/// </summary>
		public float CenterX { get { return MinX + 0.5f * SizeX; } set { MinX = value - 0.5f * SizeX; } }
		/// <summary>
		/// Y-coordinate of the center of the box. Setting the value will move the box, while to size will not change.
		/// </summary>
		public float CenterY { get { return MinY + 0.5f * SizeY; } set { MinY = value - 0.5f * SizeY; } }
		/// <summary>
		/// Maximal x coordinate. Setting the value will change the size of the box, while MinX and MinY will stay the same.
		/// </summary>
		public float MaxX { get { return MinX + SizeX; } set { SizeX = value - MinX; } }

		/// <summary>
		/// Maximal y coordinate. Setting the value will change the size of the box, 
		/// while <see cref="MinX"/> and <see cref="MinY"/> will stay the same.
		/// </summary>
		public float MaxY { get { return MinY + SizeY; } set { SizeY = value - MinY; } }
		/// <summary>
		/// Minimal x coordinate. Setting the value will move the box, while to size will not change.
		/// </summary>
		public float MinX { get; set; }
		/// <summary>
		/// Minimal y coordinate. Setting the value will move the box, while to size will not change.
		/// </summary>
		public float MinY { get; set; }
		/// <summary>
		/// Size of the box in x-direction. Setting the value will change the size of the box, 
		/// while <see cref="MinX"/> and <see cref="MinY"/> will stay the same.
		/// </summary>
		public float SizeX { get; set; }
		/// <summary>
		/// Size of the box in y-direction. Setting the value will change the size of the box, 
		/// while <see cref="MinX"/> and <see cref="MinY"/> will stay the same.
		/// </summary>
		public float SizeY { get; set; }

		/// <summary>
		/// Compare two rectangles for equal size and position
		/// </summary>
		/// <param name="a">First rectangle to compare</param>
		/// <param name="b">Second rectangle to compare</param>
		/// <returns>true when size and position are the same</returns>
		public static bool operator==(Box2D a, Box2D b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Compare two rectangles for equal size and position
		/// </summary>
		/// <param name="a">First rectangle to compare</param>
		/// <param name="b">Second rectangle to compare</param>
		/// <returns>false when size and position are the same</returns>
		public static bool operator !=(Box2D a, Box2D b)
		{
			return !a.Equals(b);
		}

		/// <summary>
		/// Tests two rectangles for equal size and position
		/// </summary>
		/// <param name="other">second rectangle</param>
		/// <returns>False if not a rectangle</returns>
		public bool Equals(IReadOnlyBox2D other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			return MinX == other.MinX && MinY == other.MinY && SizeX == other.SizeX && SizeY == other.SizeY;
		}

		/// <summary>
		/// Test if two rectangles have equal position and size
		/// </summary>
		/// <param name="other">rectangle to compare to</param>
		/// <returns>False if not a rectangle</returns>
		public override bool Equals(object other)
		{
			return Equals(other as IReadOnlyBox2D);
		}

		/// <summary>
		/// A hash code produced out of hash codes of <see cref="MinX"/>, <see cref="MinY"/>, <see cref="SizeX"/>, <see cref="SizeY"/>.
		/// </summary>
		/// <returns>A hash code produced out of hash codes of <see cref="MinX"/>, <see cref="MinY"/>, <see cref="SizeX"/>, <see cref="SizeY"/>.</returns>
		public override int GetHashCode()
		{
			unchecked
			{//TODO: hashcode.combine
				var hashCode = MinX.GetHashCode();
				hashCode = (hashCode * 397) ^ MinY.GetHashCode();
				hashCode = (hashCode * 397) ^ SizeX.GetHashCode();
				hashCode = (hashCode * 397) ^ SizeY.GetHashCode();
				return hashCode;
			}
		}

		/// <summary>
		/// Returns a string of format (<see cref = "MinX" />;<see cref = "MinY" />;
		/// <see cref = "SizeX" />;<see cref = "SizeY" />)
		/// </summary>
		/// <returns>
		/// String of format (<see cref = "MinX" />;<see cref = "MinY" />;
		/// <see cref = "SizeX" />;<see cref = "SizeY" />)
		/// </returns>
		public override string ToString()
		{
			return $"({MinX};{MinY};{SizeX};{SizeY})";
		}
	}
}
