using Zenseless.ExampleFramework;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;
using Zenseless.HLGL;

namespace Example
{
	/// <summary>
	/// shows side scrolling setup by manipulating texture coordinates
	/// </summary>
	class MyVisual
	{
		private ITexture texBackground;
		private ITexture texPlayer;
		private Box2D texCoord = new Box2D(0, 0, 0.3f, 1);

		private MyVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			texPlayer = contentLoader.Load<ITexture2D>("bird1");
			texBackground = contentLoader.Load<ITexture2D>("forest");
			//texBackground = contentLoader.Load<ITexture2D>("landscape");
			//set how texture coordinates outside of [0..1] are handled
			texBackground.WrapFunction = TextureWrapFunction.MirroredRepeat;

			//for transparency in textures
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);
			//draw background with changing texture coordinates
			DrawTexturedRect(new Box2D(-1, -1, 2, 2), texBackground, texCoord);

			DrawTexturedRect(new Box2D(-.25f, -.1f, .2f, .2f), texPlayer, new Box2D(0, 0, 1, 1)); // draw player
		}

		private void Update(float updatePeriod)
		{
			texCoord.MinX += updatePeriod * 0.1f; //scroll texture coordinates
		}

		private static void DrawTexturedRect(IReadOnlyBox2D rect, ITexture tex, IReadOnlyBox2D texCoords)
		{
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(texCoords.MinX, texCoords.MinY); GL.Vertex2(rect.MinX, rect.MinY);
			GL.TexCoord2(texCoords.MaxX, texCoords.MinY); GL.Vertex2(rect.MaxX, rect.MinY);
			GL.TexCoord2(texCoords.MaxX, texCoords.MaxY); GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.TexCoord2(texCoords.MinX, texCoords.MaxY); GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
			tex.Deactivate();
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += visual.Render;
			window.Update += visual.Update;
			window.Run();
			window.Dispose();
		}
	}
}