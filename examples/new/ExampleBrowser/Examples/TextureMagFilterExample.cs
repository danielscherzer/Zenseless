namespace ExampleBrowser
{
	using OpenTK.Graphics.OpenGL;
	using System.ComponentModel.Composition;
	using System.Drawing;
	using Zenseless.Base;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	/// <summary>
	/// Compares texture magnification filter methods
	/// </summary>
	[ExampleDisplayName]
	[Export(typeof(IExample))]
	class TextureMagFilterExample : IExample
	{
		private ITexture texBackground;
		private Box2D texCoord = new Box2D(0, 0, 1, 1);
		private float scaleFactor = 1f;

		[ImportingConstructor]
		private TextureMagFilterExample([Import] IContentLoader contentLoader)
		{
			texBackground = contentLoader.Load<ITexture2D>("mountains");
			texBackground.WrapFunction = TextureWrapFunction.ClampToBorder;
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw with different filter modes - defines how texture colors are mapped to pixel colors
			texBackground.Filter = TextureFilterMode.Nearest; //filter by taking the nearest texel's color as a pixels color
			DrawTexturedRect(new Box2D(-1, -1, 1, 2), texBackground, texCoord);
			texBackground.Filter = TextureFilterMode.Linear; //filter by calculating the pixels color as an weighted average of the neighboring texel's colors
			DrawTexturedRect(new Box2D(0, -1, 1, 2), texBackground, texCoord);
		}

		private static void DrawTexturedRect(IReadOnlyBox2D rect, ITexture tex, IReadOnlyBox2D texCoords)
		{
			tex.Activate();
			rect.DrawTexturedRect(texCoords);
			tex.Deactivate();
		}

		public void Update(ITime time)
		{
			if (texCoord.SizeX > 0.99f || texCoord.SizeX < 0.05f) scaleFactor = -scaleFactor;
			float factor = 1 + scaleFactor * time.DeltaTime;
			texCoord.SizeX *= factor;
			texCoord.SizeY *= factor;
			texCoord.CenterX = 0.5f;
			texCoord.CenterY = 0.5f;
		}
	}
}