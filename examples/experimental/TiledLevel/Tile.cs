using Zenseless.Geometry;

namespace Example
{
	public class Tile : ITile
	{
		public Tile(IReadOnlyBox2D bounds, uint type, bool walkable)
		{
			Bounds = bounds;
			Type = type;
			Walkable = walkable;
		}

		public IReadOnlyBox2D Bounds { get; }
		public uint Type { get; }
		public bool Walkable { get; }
	}
}
