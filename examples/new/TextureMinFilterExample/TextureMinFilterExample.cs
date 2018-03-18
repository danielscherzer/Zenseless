using Zenseless.ExampleFramework;
using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.HLGL;

namespace Example
{
	/// <summary>
	/// Compares texture minification filter methods
	/// </summary>
	class MyVisual
	{
		private ITexture texBackground;
		private Box2D texCoord = new Box2D(0, 0, 1, 1);
		private float scaleFactor = 1f;

		private MyVisual(IContentLoader contentLoader)
		{
			texBackground = contentLoader.Load<ITexture2D>("mountains");
			texBackground.WrapFunction = TextureWrapFunction.Repeat;
			GL.Enable(EnableCap.Texture2D); //todo: only for non shader pipeline relevant -> remove at some point
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw with different filter modes - defines how texture colors are mapped to pixel colors
			texBackground.Filter = TextureFilterMode.Linear; //filter by taking the nearest texel's color as a pixels color
			DrawTexturedRect(new Box2D(-1, -1, 1, 2), texBackground, texCoord);
			texBackground.Filter = TextureFilterMode.Mipmap; //filter by calculating the pixels color as a weighted average of the neighboring texel's colors
			DrawTexturedRect(new Box2D(0, -1, 1, 2), texBackground, texCoord);
		}

		private void Update(float updatePeriod)
		{
			if (texCoord.SizeX > 200f || texCoord.SizeX < 1f) scaleFactor = -scaleFactor;
			float factor = scaleFactor * updatePeriod;
			texCoord.SizeX *= 1 + factor;
			texCoord.SizeY *= 1 + factor;
			//texCoord.CenterX += factor * 0.1f;
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