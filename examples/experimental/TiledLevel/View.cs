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
		private readonly Dictionary<int, ITexture2D> texTileSets = new Dictionary<int, ITexture2D>();
		private readonly List<Grid<int>> layerGrid;
		private readonly Vector2 gridSize;
		private float aspect;

		public View(IContentLoader contentLoader, IRenderState renderState, Dictionary<int, string> tileSprites, List<Grid<int>> gridLayers)
		{
			if (contentLoader == null)
			{
				throw new ArgumentNullException(nameof(contentLoader));
			}

			if (renderState == null)
			{
				throw new ArgumentNullException(nameof(renderState));
			}

			if (tileSprites == null)
			{
				throw new ArgumentNullException(nameof(tileSprites));
			}

			renderState.Set(BlendStates.AlphaBlend);
			foreach(var sprite in tileSprites)
			{
				var tex = contentLoader.Load<ITexture2D>(sprite.Value);
				tex.WrapFunction = TextureWrapFunction.ClampToEdge;
				tex.Filter = TextureFilterMode.Nearest;
				texTileSets[sprite.Key] = tex;
			}
			layerGrid = gridLayers ?? throw new ArgumentNullException(nameof(gridLayers));
			var grid = layerGrid[0];
			gridSize = new Vector2(grid.Width, grid.Height);
		}

		public float Zoom { get; set; } = 1f;

		internal void Draw(IReadOnlyBox2D player)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			var playerCenter = player.GetCenter();
			var zoom = Zoom * .1f;
			GL.LoadIdentity();
			GL.Scale(aspect, 1f, 1f);
			GL.Scale(zoom, zoom, 1f);
			GL.Translate(-playerCenter.X, -playerCenter.Y, 0f);

			var delta = new Vector2(1f / (aspect * zoom), 1f / zoom); //inverse of transformations
			var min = playerCenter - delta;
			var max = playerCenter + delta;

			GL.Enable(EnableCap.Texture2D);
			DrawVisibleTiles(min, max);
			GL.Disable(EnableCap.Texture2D);
			DrawRect(player, new Color4(1f, 1f, 1f, 0.7f));
		}

		private void DrawVisibleTiles(in Vector2 min, in Vector2 max)
		{
			var tileMin = MathHelper.Clamp(MathHelper.Truncate(min), Vector2.Zero, gridSize);
			var tileMax = MathHelper.Clamp(MathHelper.Ceiling(max), Vector2.Zero, gridSize);

			foreach (var layer in layerGrid)
			{
				for (int x = (int)tileMin.X; x < (int)tileMax.X; ++x)
				{
					for (int y = (int)tileMin.Y; y < (int)tileMax.Y; ++y)
					{
						DrawTile(layer, x, y);
					}
				}
			}
		}

		private void DrawTile(Grid<int> layer, int tileX, int tileY)
		{
			var gid = layer.GetElement(tileX, tileY);
			if (0 != gid)
			{
				 var tex = texTileSets[gid];
				var x = tileX;
				var y = tileY;
				var bounds = new Box2D(x, y, 1f, 1f);
				tex.Activate();
				var d = 0f; //1f / 8;
				var texCoord = new Box2D(d, d, 1 - d, 1 - d);
				DrawTools.DrawTexturedRect(bounds, texCoord);
			}
		}

		internal void Resize(int width, int height)
		{
			aspect = height / (float)width;
		}

		private static void DrawRect(IReadOnlyBox2D rectangle, Color4 color)
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