namespace Example
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using TiledSharp;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class TileMap
	{
		public TileMap()
		{
			var map = new TmxMap(@"D:\Daten\tiled\grass4x\grass4x.tmx");
			var tileSize = new Vector2(1f / (map.Width - 1), 1f / (map.Height - 1));
			var tileSet = map.Tilesets[0];

		   var walkable = ToHashSet(from tile in tileSet.Tiles where bool.Parse(tile.Properties["Walkable"]) select tile.Id);
			foreach (var layer in map.Layers)
			{
				foreach (var tile in layer.Tiles)
				{
					if (0 == tile.Gid) continue;
					var x = tile.X / (float)map.Width;
					var y = (map.Height - tile.Y - 1) / (float)map.Height;
					var geom = new Box2D(x, y, tileSize.X, tileSize.Y);
					tiles.Add(new Tile(geom, (uint) tile.Gid - 1, walkable.Contains(tile.Gid - 1)));
				}
			}

			SpriteSheetName = tileSet.Name;
			var columns = (uint)tileSet.Columns.Value;
			var rows = (uint)(tileSet.TileCount.Value / tileSet.Columns.Value);
			TileTypes = new IReadOnlyBox2D[tileSet.Tiles.Count];
			foreach (var tile in tileSet.Tiles)
			{
				var tex = SpriteSheet.CalcSpriteTexCoords((uint)tile.Id, columns, rows);
				TileTypes[(uint)tile.Id] = tex;
			}

			var start = map.ObjectGroups[0].Objects[0];
			Start = new Vector2((float)(start.X / ((map.Width) * map.TileWidth)), 1f - (float)(start.Y / (map.Width * map.TileHeight)));
		}

		private List<Tile> tiles = new List<Tile>();

		public IEnumerable<Tile> Tiles => tiles;
		public string SpriteSheetName { get; }
		public Vector2 Start { get; }

		public IReadOnlyBox2D[] TileTypes { get; }

		public static HashSet<T> ToHashSet<T>(IEnumerable<T> source, IEqualityComparer<T> comparer = null)
		{
			return new HashSet<T>(source, comparer);
		}
	}
}
