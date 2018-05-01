namespace SpaceInvadersMvc
{
	using OpenTK.Graphics.OpenGL;
	using System.Collections.Generic;
	using System.Drawing;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class View
	{
		public View(IContentLoader contentLoader)
		{
			//load textures from embedded resources
			texPlayer = contentLoader.Load<ITexture2D>("blueships1");
			texEnemy = contentLoader.Load<ITexture2D>("redship4");
			texBullet = contentLoader.Load<ITexture2D>("blueLaserRay");

			GL.Enable(EnableCap.Blend); //enable blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.Enable(EnableCap.Texture2D); //enable texturing
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Color3(Color.White);
		}

		public void DrawEnemies(IEnumerable<IReadOnlyBox2D> enemies) => DrawSpriteBatch(enemies, texEnemy);
		public void DrawBullets(IEnumerable<IReadOnlyBox2D> bullets) => DrawSpriteBatch(bullets, texBullet);

		public void DrawPlayer(IReadOnlyBox2D player)
		{
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
