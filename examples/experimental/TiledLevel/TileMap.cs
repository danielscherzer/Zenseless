namespace Example
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Numerics;
	using System.Reflection;
	using TiledSharp;

	public class TileMap
	{
		public TileMap(string fileName)
		{
			map = new TmxMap(fileName);
		}

		private readonly TmxMap map;

		public Grid<bool> ExtractCollisionGrid()
		{
			var tileSet = map.Tilesets[0];
			var walkableIds = new HashSet<int>(from tile in tileSet.Tiles where !tile.Properties.ContainsKey("forbidden") select tile.Id);
			var gridIsWalkable = new Grid<bool>(map.Width, map.Height);
			gridIsWalkable.Clear(true);
			foreach (var layer in map.Layers)
			{
				foreach (var tile in layer.Tiles)
				{
					if (0 == tile.Gid) continue;
					var isWalkable = walkableIds.Contains(tile.Gid - 1);
					var oldValue = gridIsWalkable.GetElement(tile.X, map.Height - tile.Y - 1);
					gridIsWalkable.SetElement(tile.X, map.Height - tile.Y - 1, isWalkable & oldValue);
				}
			}
			return gridIsWalkable;
		}

		public List<Grid<int>> ExtractViewLayerGrids()
		{
			var gridLayers = new List<Grid<int>>();
			foreach (var layer in map.Layers)
			{
				var grid = new Grid<int>(map.Width, map.Height);
				foreach (var tile in layer.Tiles)
				{
					grid.SetElement(tile.X, map.Height - tile.Y - 1, tile.Gid);
				}
				gridLayers.Add(grid);
			}
			return gridLayers;
		}

		public Dictionary<int, string> ExtractTileSprites()
		{
			var tileTypes = new Dictionary<int, string>();
			var dir = map.TmxDirectory + Path.DirectorySeparatorChar;
			foreach (var tileSet in map.Tilesets)
			{
				foreach (var tile in tileSet.Tiles)
				{
					var name = tile.Image.Source.Replace(dir, string.Empty);
					name = name.Replace(Path.AltDirectorySeparatorChar, '.');
					tileTypes.Add(tileSet.FirstGid + tile.Id, name);
				}
			}
			return tileTypes;
		}

		public Vector2 ExtractStart()
		{
			var start = map.ObjectGroups[0].Objects[0];
			return new Vector2((float)(start.X / map.TileWidth), map.Height - (float)(start.Y / map.TileHeight));
		}
	}
}
