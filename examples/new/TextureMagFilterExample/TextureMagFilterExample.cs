using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.ExampleFramework;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	/// <summary>
	/// Compares texture magnification filter methods
	/// </summary>
	class MyVisual
	{
		private ITexture texBackground;
		private Box2D texCoord = new Box2D(0, 0, 1, 1);
		private float scaleFactor = 1f;

		private MyVisual(IContentLoader contentLoader)
		{
			texBackground = contentLoader.Load<ITexture2D>("mountains");
			texBackground.WrapFunction = TextureWrapFunction.ClampToBorder;
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		private void Render()
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

		private void Update(float updatePeriod)
		{
			if (texCoord.SizeX > 0.99f || texCoord.SizeX < 0.05f) scaleFactor = -scaleFactor;
			float factor = 1 + scaleFactor * updatePeriod;
			texCoord.SizeX *= factor;
			texCoord.SizeY *= factor;
			texCoord.CenterX = 0.5f;
			texCoord.CenterY = 0.5f;
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual(window.ContentLoader);
			window.Render += visual.Render;
			window.Update += visual.Update;
			window.Run();
		}

		private static void DrawTexturedRect(IReadOnlyBox2D rect, ITexture tex, IReadOnlyBox2D texCoords)
		{
			tex.Activate();
			rect.DrawTexturedRect(texCoords);
			tex.Deactivate();
		}
	}
}