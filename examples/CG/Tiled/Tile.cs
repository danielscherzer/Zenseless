using Zenseless.Geometry;

namespace Example
{
	public class Tile : ITile
	{
		public Tile(IReadOnlyBox2D bounds, IReadOnlyBox2D tex)
		{
			Bounds = bounds;
			TexCoords = tex;
		}

		public IReadOnlyBox2D Bounds { get; }
		public IReadOnlyBox2D TexCoords { get; }
	}
}
