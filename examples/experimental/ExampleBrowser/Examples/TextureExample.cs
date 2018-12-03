namespace ExampleBrowser
{
	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL;
	using System.ComponentModel.Composition;
	using System.Drawing;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	/// <summary>
	/// Example that shows loading and using textures. 
	/// It loads 2 textures: one for the background and one for a space ship.
	/// </summary>
	[Export(typeof(IExample))]
	[ExampleDisplayName]
	class TextureExample : IExample
	{
		private readonly ITexture texBackground;
		private readonly ITexture texChecker;
		private readonly ITexture texShip;

		[ImportingConstructor]
		public TextureExample([Import] IRenderState renderState, [Import] IContentLoader contentLoader)
		{
			renderState.Set(BlendStates.AlphaBlend); // for transparency in textures we use blending

			texShip = contentLoader.Load<ITexture2D>("ExampleBrowser.Content.redship4.png"); // fully qualified resource name of the content file
			texBackground = contentLoader.Load<ITexture2D>("water"); // the short case-insensitive version is also found
			texChecker = CreaterCheckerTex();
			GL.Enable(EnableCap.Texture2D);
		}

		private static ITexture CreaterCheckerTex()
		{
			var black = Color4.Black;
			var white = Color4.White;
			var data = new Color4[2, 2];
			data[0, 0] = black;
			data[0, 1] = white;
			data[1, 0] = white;
			data[1, 1] = black;
			var tex = new Texture2dGL();
			tex.LoadPixels(data, 4, true);
			tex.Filter = TextureFilterMode.Nearest;
			return tex;
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color => white == no change to texture color
			GL.Color3(Color.White);
			//draw background
			DrawTexturedRect(new Box2D(-1, -1, 2, 2), Box2D.BOX01, texBackground);
			//draw ship
			DrawTexturedRect(new Box2D(.25f, -.25f, .5f, .5f), Box2D.BOX01, texShip);
			//draw checker
			var texRect = new Box2D(0, 0, 2, 2);
			DrawTexturedRect(new Box2D(-.75f, -.25f, .5f, .5f), texRect, texChecker);
		}

		private static void DrawTexturedRect(IReadOnlyBox2D geometry, IReadOnlyBox2D texRect, ITexture texture)
		{
			//the texture has to be enabled before use
			texture.Activate();
			GL.Begin(PrimitiveType.Quads);
			//when using textures we have to set a texture coordinate for each vertex
			//by using the TexCoord command BEFORE the Vertex command
			GL.TexCoord2(texRect.MinX, texRect.MinY); GL.Vertex2(geometry.MinX, geometry.MinY);
			GL.TexCoord2(texRect.MaxX, texRect.MinY); GL.Vertex2(geometry.MaxX, geometry.MinY);
			GL.TexCoord2(texRect.MaxX, texRect.MaxY); GL.Vertex2(geometry.MaxX, geometry.MaxY);
			GL.TexCoord2(texRect.MinX, texRect.MaxY); GL.Vertex2(geometry.MinX, geometry.MaxY);
			GL.End();
			//the texture is disabled, so no other draw calls use this texture
			texture.Deactivate();
		}

		public void Update()
		{
		}
	}
}