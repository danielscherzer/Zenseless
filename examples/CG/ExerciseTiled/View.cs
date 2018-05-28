namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Collections.Generic;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	internal class View
	{
		private readonly ITexture2D texTileSet;

		public View(IContentLoader contentLoader, IRenderState renderState, string spriteSheetName)
		{
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D);
			texTileSet = contentLoader.Load<ITexture2D>(spriteSheetName);
			texTileSet.Filter = TextureFilterMode.Nearest;
		}

		internal void Draw(IEnumerable<ITile> tiles)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Translate(-1f, -1f, 0f);
			GL.Scale(2f, 2f, 1f);

			texTileSet.Activate();
			foreach (var tile in tiles)
			{
				DrawTools.DrawTexturedRect(tile.Bounds, tile.TexCoords);
			}
			texTileSet.Deactivate();
		}
	}
}