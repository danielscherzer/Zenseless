namespace Example
{
	using System;
	using System.Numerics;
	using Zenseless.Geometry;

	internal class Model
	{
		private readonly Grid<bool> walkable;
		private Box2D player;

		public Model(Vector2 start, Grid<bool> walkable)
		{
			this.walkable = walkable ?? throw new ArgumentNullException(nameof(walkable));
			player = Box2DExtensions.CreateFromCenterSize(start.X, start.Y, 1f, 1f);
		}

		public IReadOnlyBox2D Player { get => player; }

		internal void Update(float deltaX, float deltaY)
		{
			var speed = 4f;
			player.MinX += deltaX * speed;
			player.MinY += deltaY * speed;
			if (player.MinX < 0) player.MinX = 0;
			if (player.MinY < 0) player.MinY = 0;
			if (player.MaxX > walkable.Width) player.MinX = walkable.Width - player.SizeX;
			if (player.MaxY > walkable.Height) player.MinY = walkable.Height - player.SizeY;
			LevelPlayerCollision(player);
		}

		private void LevelPlayerCollision(Box2D player)
		{
			var playerLowerX = (int)player.MinX;
			var playerLowerY = (int)player.MinY;
			var playerUpperX = (int)Math.Ceiling(player.MaxX);
			var playerUpperY = (int)Math.Ceiling(player.MaxY);
			var tileBounds = Box2DExtensions.CreateFromMinSize(Vector2.Zero, Vector2.One);
			for (var x = playerLowerX; x < playerUpperX; ++x)
			{
				for (var y = playerLowerY; y < playerUpperY; ++y)
				{
					if (!walkable.GetElement(x, y))
					{
						tileBounds.MinX = x;
						tileBounds.MinY = y;
						player.UndoOverlap(tileBounds);
					}
				}
			}
		}
	}
}