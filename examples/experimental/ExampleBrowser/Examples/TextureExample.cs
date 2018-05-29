namespace ExampleBrowser
{
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.ComponentModel.Composition;
	using System.Drawing;
	using Zenseless.Patterns;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// Example that shows loading and using textures. 
	/// It loads 2 textures: one for the background and one for a space ship.
	/// </summary>
	[Export(typeof(IExample))]
	[ExampleDisplayName]
	class TextureExample : IExample
	{
		private readonly ITexture texBackground;
		private readonly ITexture texShip;

		[ImportingConstructor]
		public TextureExample([Import] IRenderState renderState, [Import] IContentLoader contentLoader)
		{
			renderState.Set(BlendStates.AlphaBlend); // for transparency in textures we use blending

			texShip = contentLoader.Load<ITexture2D>("ExampleBrowser.Content.redship4.png"); // fully qualified resource name of the content file
			texBackground = contentLoader.Load<ITexture2D>("water"); // the short case-insensitive version is also found
			GL.Enable(EnableCap.Texture2D);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color => white == no change to texture color
			GL.Color3(Color.White);
			//draw background
			DrawTexturedRect(new Box2D(-1, -1, 2, 2), texBackground);
			//draw ship
			DrawTexturedRect(new Box2D(-.25f, -.25f, .5f, .5f), texShip);
		}

		private static void DrawTexturedRect(IReadOnlyBox2D Rect, ITexture texture)
		{
			//the texture has to be enabled before use
			texture.Activate();
			GL.Begin(PrimitiveType.Quads);
			//when using textures we have to set a texture coordinate for each vertex
			//by using the TexCoord command BEFORE the Vertex command
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rect.MinX, Rect.MinY);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rect.MaxX, Rect.MinY);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rect.MaxX, Rect.MaxY);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rect.MinX, Rect.MaxY);
			GL.End();
			//the texture is disabled, so no other draw calls use this texture
			texture.Deactivate();
		}

		public void Update()
		{
		}
	}
}