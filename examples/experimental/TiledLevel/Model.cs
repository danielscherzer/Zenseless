namespace Example
{
	using System;
	using System.Numerics;
	using Zenseless.Geometry;

	internal class Model
	{
		private readonly IGrid<bool> walkable;
		private Box2D player;

		public Model(Vector2 start, IGrid<bool> walkable)
		{
			this.walkable = walkable ?? throw new ArgumentNullException(nameof(walkable));
			player = Box2DExtensions.CreateFromCenterSize(start.X, start.Y, 1f / walkable.Width, 1f / walkable.Height);
		}

		public IReadOnlyBox2D Player { get => player; }

		internal void Update(float deltaX, float deltaY)
		{
			var speed = 0.1f;
			player.MinX += deltaX * speed;
			player.MinY += deltaY * speed;
			if (player.MinX < 0) player.MinX = 0;
			if (player.MinY < 0) player.MinY = 0;
			if (player.MaxX > 1) player.MinX = 1f - player.SizeX;
			if (player.MaxY > 1) player.MinY = 1f - player.SizeY;
			LevelPlayerCollision(player);
		}

		private void LevelPlayerCollision(Box2D player)
		{
			var playerLowerX = MathHelper.FastTruncate(player.MinX * walkable.Width);
			var playerLowerY = MathHelper.FastTruncate(player.MinY * walkable.Height);
			var playerUpperX = (int)Math.Ceiling(player.MaxX * walkable.Width);
			var playerUpperY = (int)Math.Ceiling(player.MaxY * walkable.Height);
			var tileSize = new Vector2(1f / walkable.Width, 1f / walkable.Height);
			var tileBounds = Box2DExtensions.CreateFromMinSize(Vector2.Zero, tileSize);
			for (var x = playerLowerX; x < playerUpperX; ++x)
			{
				for (var y = playerLowerY; y < playerUpperY; ++y)
				{
					if (!walkable.GetElement(x, y))
					{
						tileBounds.MinX = x * tileSize.X;
						tileBounds.MinY = y * tileSize.Y;
						player.UndoOverlap(tileBounds);
					}
				}
			}
		}
	}
}