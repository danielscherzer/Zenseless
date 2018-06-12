namespace Example
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Numerics;
	using TiledSharp;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class TileMap
	{
		public TileMap()
		{
			map = new TmxMap(@"D:\Daten\tiled\grass4x\grass4x.tmx");
			var tileSize = new Vector2(1f / (map.Width - 1), 1f / (map.Height - 1));

			foreach (var layer in map.Layers)
			{
				foreach (var tile in layer.Tiles)
				{
					if (0 == tile.Gid) continue;
					var x = tile.X / (float)map.Width;
					var y = (map.Height - tile.Y - 1) / (float)map.Height;
					var geom = new Box2D(x, y, tileSize.X, tileSize.Y);
					tiles.Add(new Tile(geom, (uint) tile.Gid - 1));
				}
			}

			var tileSet = map.Tilesets[0];
			SpriteSheetName = Path.GetFileName(tileSet.Image.Source);
			var columns = (uint)tileSet.Columns.Value;
			var rows = (uint)(tileSet.TileCount.Value / tileSet.Columns.Value);
			TileTypes = new IReadOnlyBox2D[tileSet.Tiles.Count];
			foreach (var tile in tileSet.Tiles)
			{
				var tex = SpriteSheet.CalcSpriteTexCoords((uint)tile.Id, columns, rows);
				TileTypes[(uint)tile.Id] = tex;
			}
		}

		private List<Tile> tiles = new List<Tile>();
		private readonly TmxMap map;

		public IEnumerable<Tile> Tiles => tiles;

		public IGrid<bool> ExtractCollisionGrid()
		{
			var tileSet = map.Tilesets[0];
			var walkableIds = new HashSet<int>(from tile in tileSet.Tiles where bool.Parse(tile.Properties["Walkable"]) select tile.Id);
			var grid = new Grid<bool>(map.Width, map.Height);
			grid.Clear(true);
			foreach (var layer in map.Layers)
			{
				foreach (var tile in layer.Tiles)
				{
					if (0 == tile.Gid) continue;
					var isWalkable = walkableIds.Contains(tile.Gid - 1);
					var oldValue = grid.GetElement(tile.X, map.Height - tile.Y - 1);
					grid.SetElement(tile.X, map.Height - tile.Y - 1, isWalkable & oldValue);
				}
			}
			return grid;
		}

		public IGrid<int> ExtractViewGrid()
		{
			var tileSet = map.Tilesets[0];
			var grid = new Grid<int>(map.Width, map.Height);
			foreach (var layer in map.Layers)
			{
				foreach (var tile in layer.Tiles)
				{
					grid.SetElement(tile.X, map.Height - tile.Y - 1, tile.Gid);
				}
			}
			return grid;
		}

		public string SpriteSheetName { get; }

		public Vector2 ExtractStart()
		{
			var start = map.ObjectGroups[0].Objects[0];
			return new Vector2((float)(start.X / ((map.Width) * map.TileWidth)), 1f - (float)(start.Y / (map.Width * map.TileHeight)));
		}

		public IReadOnlyBox2D[] TileTypes { get; }
	}
}
