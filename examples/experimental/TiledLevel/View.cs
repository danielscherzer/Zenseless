namespace Example
{
	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.Collections.Generic;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	internal class View
	{
		private readonly ITexture2D texTileSet;
		private readonly IReadOnlyBox2D[] tileTypes;
		private float aspect;

		public View(IContentLoader contentLoader, IRenderState renderState, IReadOnlyBox2D[] tileTypes, string spriteSheetName)
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
			texTileSet.Filter = TextureFilterMode.Linear;
			texTileSet.WrapFunction = TextureWrapFunction.ClampToEdge;
			this.tileTypes = tileTypes ?? throw new ArgumentNullException(nameof(tileTypes));
		}

		public float Zoom { get; set; } = 1f;
		internal void Draw(IEnumerable<ITile> tiles, IReadOnlyBox2D player)
		{
			var zoom = Zoom * 4f;
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Scale(aspect, 1f, 1f);
			GL.Scale(zoom, zoom, 1f);
			GL.Translate(-player.CenterX, -player.CenterY, 0f);

			texTileSet.Activate();
			foreach (var tile in tiles)
			{
				var texCoords = tileTypes[tile.Type];
				DrawTools.DrawTexturedRect(tile.Bounds, texCoords);
				//if (!tile.Walkable)
				//{
				//	DrawRect(tile.Bounds, new Color4(1f, 0, 0, .25f));
				//}
			}
			texTileSet.Deactivate();
			DrawRect(player, new Color4(1f, 1f, 1f, 0.7f));
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