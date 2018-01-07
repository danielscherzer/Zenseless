namespace Zenseless.Geometry
{
	/// <summary>
	/// Represents a 3D mutable axis-aligned bounding box
	/// </summary>
	public class Box3D
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Box3D"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <param name="sizeX">The size x.</param>
		/// <param name="sizeY">The size y.</param>
		/// <param name="sizeZ">The size z.</param>
		public Box3D(float x, float y, float z, float sizeX, float sizeY, float sizeZ)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.SizeX = sizeX;
			this.SizeY = sizeY;
			this.SizeZ = sizeZ;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Box3D"/> class.
		/// </summary>
		/// <param name="box">The box.</param>
		public Box3D(Box3D box)
		{
			this.X = box.X;
			this.Y = box.Y;
			this.Z = box.Z;
			this.SizeX = box.SizeX;
			this.SizeY = box.SizeY;
			this.SizeZ = box.SizeZ;
		}

		/// <summary>
		/// Gets or sets the size x.
		/// </summary>
		/// <value>
		/// The size x.
		/// </value>
		public float SizeX { get; set; }
		/// <summary>
		/// Gets or sets the size y.
		/// </summary>
		/// <value>
		/// The size y.
		/// </value>
		public float SizeY { get; set; }
		/// <summary>
		/// Gets or sets the size z.
		/// </summary>
		/// <value>
		/// The size z.
		/// </value>
		public float SizeZ { get; set; }

		//public Vector3 Corner;

		//public float X { get { return Corner.X; } set { Corner.X = value; } }
		//public float Y { get { return Corner.Y; } set { Corner.Y = value; } }
		//public float Z { get { return Corner.Z; } set { Corner.Z = value; } }
		/// <summary>
		/// Gets or sets the x.
		/// </summary>
		/// <value>
		/// The x.
		/// </value>
		public float X { get; set; }
		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		/// <value>
		/// The y.
		/// </value>
		public float Y { get; set; }
		/// <summary>
		/// Gets or sets the z.
		/// </summary>
		/// <value>
		/// The z.
		/// </value>
		public float Z { get; set; }

		/// <summary>
		/// Gets or sets the center x.
		/// </summary>
		/// <value>
		/// The center x.
		/// </value>
		public float CenterX { get { return X + 0.5f * SizeX; } set { X = value - 0.5f * SizeX; } }
		/// <summary>
		/// Gets or sets the center y.
		/// </summary>
		/// <value>
		/// The center y.
		/// </value>
		public float CenterY { get { return Y + 0.5f * SizeY; } set { Y = value - 0.5f * SizeY; } }
		/// <summary>
		/// Gets or sets the center z.
		/// </summary>
		/// <value>
		/// The center z.
		/// </value>
		public float CenterZ { get { return Y + 0.5f * SizeZ; } set { Y = value - 0.5f * SizeZ; } }

		/// <summary>
		/// Intersects es the specified box.
		/// </summary>
		/// <param name="box">The box.</param>
		/// <returns></returns>
		public bool Intersects(Box3D box)
		{
			if (box is null) return false;
			bool noXintersect = (MaxX < box.X) || (X > box.MaxX);
			bool noYintersect = (MaxY < box.Y) || (Y > box.MaxY);
			bool noZintersect = (MaxZ < box.Z) || (Z > box.MaxZ);
			return !(noXintersect || noYintersect || noZintersect);
		}

		/// <summary>
		/// Determines whether [contains] [the specified x].
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <returns>
		///   <c>true</c> if [contains] [the specified x]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(float x, float y, float z)
		{
			if (x < X || MaxX < x) return false;
			if (y < Y || MaxY < y) return false;
			if (z < Z || MaxZ < z) return false;
			return true;
		}

		/// <summary>
		/// Determines whether [contains] [the specified box].
		/// </summary>
		/// <param name="box">The box.</param>
		/// <returns>
		///   <c>true</c> if [contains] [the specified box]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(Box3D box)
		{
			if (X < box.X) return false;
			if (MaxX > box.MaxX) return false;
			if (Y < box.Y) return false;
			if (MaxY > box.MaxY) return false;
			if (Z < box.Z) return false;
			if (MaxZ > box.MaxZ) return false;
			return true;
		}

		/// <summary>
		/// Gets or sets the maximum x.
		/// </summary>
		/// <value>
		/// The maximum x.
		/// </value>
		public float MaxX { get { return X + SizeX; } set { X = value - SizeX; } }
		/// <summary>
		/// Gets or sets the maximum y.
		/// </summary>
		/// <value>
		/// The maximum y.
		/// </value>
		public float MaxY { get { return Y + SizeY; } set { Y = value - SizeY; } }
		/// <summary>
		/// Gets or sets the maximum z.
		/// </summary>
		/// <value>
		/// The maximum z.
		/// </value>
		public float MaxZ { get { return Z + SizeZ; } set { Z = value - SizeZ; } }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return '(' + X.ToString() + ';' + Y.ToString() + ';' + Z.ToString() + ';' + SizeX.ToString() + ';' + SizeY.ToString() + ';' + SizeZ.ToString() + ')';
		}

	}
}
