namespace Example
{
	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	internal class View
	{
		private readonly ITexture2D texTileSet;
		private readonly IReadOnlyBox2D[] tileTypes;
		private readonly IGrid<int> grid;
		private readonly Vector2 tileSize;
		private float aspect;

		public View(IContentLoader contentLoader, IRenderState renderState, IReadOnlyBox2D[] tileTypes, string spriteSheetName, IGrid<int> grid)
		{
			if (contentLoader == null)
			{
				throw new ArgumentNullException(nameof(contentLoader));
			}

			if (renderState == null)
			{
				throw new ArgumentNullException(nameof(renderState));
			}

			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D);
			texTileSet = contentLoader.Load<ITexture2D>(spriteSheetName);
			texTileSet.Filter = TextureFilterMode.Nearest;
			texTileSet.WrapFunction = TextureWrapFunction.ClampToEdge;
			this.tileTypes = tileTypes ?? throw new ArgumentNullException(nameof(tileTypes));
			this.grid = grid ?? throw new ArgumentNullException(nameof(grid));
			tileSize = new Vector2(1f / grid.Width, 1f / grid.Height);
		}

		public float Zoom { get; set; } = 1f;

		internal void Draw(IEnumerable<ITile> tiles, IReadOnlyBox2D player)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			DrawVisibleTiles(player);
			//foreach (var tile in tiles)
			//{
			//	var texCoords = tileTypes[tile.Type];
			//	DrawTools.DrawTexturedRect(tile.Bounds, texCoords);
			//}
			DrawRect(player, new Color4(1f, 1f, 1f, 0.7f));
		}

		private void DrawVisibleTiles(IReadOnlyBox2D player)
		{
			var zoom = Zoom * 8f;
			GL.LoadIdentity();
			GL.Scale(aspect, 1f, 1f);
			GL.Scale(zoom, zoom, 1f);
			GL.Translate(-player.CenterX, -player.CenterY, 0f);

			var playerCenter = player.GetCenter();
			var size = new Vector2(grid.Width, grid.Height);
			var delta = new Vector2(1f / (aspect * zoom), 1f / zoom); //inverse of transformations
			var min = (playerCenter - delta) * size;
			var max = (playerCenter + delta) * size;
			min = MathHelper.Clamp(MathHelper.Truncate(min), Vector2.Zero, size);
			max = MathHelper.Clamp(MathHelper.Ceiling(max), Vector2.Zero, size);

			texTileSet.Activate();
			for (int x = (int)min.X; x < (int)max.X; ++x)
			{
				for (int y = (int)min.Y; y < (int)max.Y; ++y)
				{
					DrawTile(x, y);
				}
			}
			texTileSet.Deactivate();
		}

		private void DrawTile(int tileX, int tileY)
		{
			var gid = grid.GetElement(tileX, tileY);
			if (0 != gid)
			{
				var texCoords = tileTypes[gid - 1];
				var x = tileX * tileSize.X;
				var y = tileY * tileSize.Y;
				var bounds = new Box2D(x, y, tileSize.X, tileSize.Y);
				DrawTools.DrawTexturedRect(bounds, texCoords);
			}
		}

		internal void Resize(int width, int height)
		{
			aspect = height / (float)width;
		}

		private void DrawRect(IReadOnlyBox2D rectangle, Color4 color)
		{
			GL.Color4(color);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(rectangle.MinX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.Vertex2(rectangle.MinX, rectangle.MaxY);
			GL.End();
			GL.Color4(Color4.White);
		}
	}
}