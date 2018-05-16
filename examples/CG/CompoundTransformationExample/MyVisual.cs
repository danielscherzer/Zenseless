namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Collections.Generic;
	using System.Drawing;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class MyVisual
	{
		private readonly ITexture texEarth;
		private readonly ITexture texGreen;

		public MyVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new ClearColorState(1, 1, 1, 1)); //background clear color
			renderState.Set(BlendStates.AlphaBlend); //for transparency in textures we use blending
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point

			texEarth = contentLoader.Load<ITexture2D>("earth");
			texGreen = contentLoader.Load<ITexture2D>("planet");
		}

		public void Render(in IReadOnlyBox2D earth, IEnumerable<IReadOnlyBox2D> birds)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawTexturedRect(earth, texGreen);
			foreach (var bird in birds)
			{
				DrawTexturedRect(bird, texEarth);
			}
		}

		private static void DrawTexturedRect(IReadOnlyBox2D Rectangle, ITexture tex)
		{
			GL.Color3(Color.White);
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rectangle.MinX, Rectangle.MinY);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rectangle.MaxX, Rectangle.MinY);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rectangle.MaxX, Rectangle.MaxY);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rectangle.MinX, Rectangle.MaxY);
			GL.End();
			tex.Deactivate();
		}
	}
}