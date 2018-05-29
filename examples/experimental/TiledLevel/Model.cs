using System;
using System.Collections.Generic;
using System.Linq;
using Zenseless.Geometry;

namespace Example
{
	internal class Model
	{
		private readonly IEnumerable<ITile> tiles;
		private Box2D player = new Box2D(.5f, .5f, 0.01f, 0.1f);

		public Model(IEnumerable<ITile> tiles)
		{
			this.tiles = tiles;
			var first = tiles.First();
			player.SizeX = first.Bounds.SizeX;
			player.SizeY = first.Bounds.SizeY;
		}

		public IReadOnlyBox2D Player { get => player; }

		internal void Update(float deltaX, float deltaY)
		{
			var speed = 0.3f;
			player.MinX += deltaX * speed;
			player.MinY += deltaY * speed;
			foreach (var tile in tiles)
			{
				if (!tile.Walkable)
				{
					if (player.Intersects(tile.Bounds))
					{
						player.UndoOverlap(tile.Bounds);
					}
				}
			}
		}
	}
}