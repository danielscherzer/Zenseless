namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using TiledSharp;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class TileMap
	{
		public TileMap()
		{
			var map = new TmxMap(@"D:\Daten\tiled\grass\unbenannt.tmx");
			var tileSize = new Vector2(1f / (map.Width - 1), 1f / (map.Height - 1));
			var tileSet = map.Tilesets[0];
			SpriteSheetName = tileSet.Name;
			var columns = (uint)tileSet.Columns.Value;
			var rows = (uint)(tileSet.TileCount.Value / tileSet.Columns.Value);
			foreach (var tile in map.Layers[0].Tiles)
			{
				if (0 == tile.Gid) continue;
				var x = tile.X / (float)map.Width;
				var y = (map.Height - tile.Y - 1) / (float)map.Height;
				var geom = new Box2D(x, y, tileSize.X, tileSize.Y);
				var tex = SpriteSheet.CalcSpriteTexCoords((uint)tile.Gid - 1, columns, rows);
				tiles.Add(new Tile(geom, tex));
			}
		}

		private List<Tile> tiles = new List<Tile>();

		public IEnumerable<Tile> Tiles => tiles;
		public string SpriteSheetName { get; }
	}
}
