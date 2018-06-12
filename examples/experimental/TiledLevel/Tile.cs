using Zenseless.Geometry;

namespace Example
{
	public class Tile : ITile
	{
		public Tile(IReadOnlyBox2D bounds, uint type)
		{
			Bounds = bounds;
			Type = type;
		}

		public IReadOnlyBox2D Bounds { get; }
		public uint Type { get; }
	}
}
