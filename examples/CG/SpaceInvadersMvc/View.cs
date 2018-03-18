using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace SpaceInvadersMvc
{

	public class View
	{
		public View(IRenderState renderState, IContentLoader contentLoader)
		{
			texPlayer = contentLoader.Load<ITexture2D>("blueships1");
			texEnemy = contentLoader.Load<ITexture2D>("redship4");
			texBullet = contentLoader.Load<ITexture2D>("blueLaserRay");

			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		public void DrawScreen(IEnumerable<IReadOnlyBox2D> enemies, IEnumerable<IReadOnlyBox2D> bullets, IReadOnlyBox2D player)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Color3(Color.White);

			DrawSpriteBatch(enemies, texEnemy);
			DrawSpriteBatch(bullets, texBullet);
			texPlayer.Activate();
			Draw(player);
			texPlayer.Deactivate();
		}

		private readonly ITexture texEnemy;
		private readonly ITexture texPlayer;
		private readonly ITexture texBullet;

		private static void Draw(IReadOnlyBox2D rectangle)
		{
			GL.Begin(PrimitiveType.Quads);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(rectangle.MinX, rectangle.MinY);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(rectangle.MaxX, rectangle.MinY);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(rectangle.MinX, rectangle.MaxY);
			GL.End();
		}

		private static void DrawSpriteBatch(IEnumerable<IReadOnlyBox2D> bounds, ITexture texture)
		{
			texture.Activate();
			foreach (var enemy in bounds)
			{
				Draw(enemy);
			}
			texture.Deactivate();
		}
	}
}
