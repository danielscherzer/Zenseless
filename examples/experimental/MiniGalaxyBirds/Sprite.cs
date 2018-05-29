using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Zenseless.HLGL;

namespace MiniGalaxyBirds
{
	public class Sprite : IDrawable
	{
		public Sprite(ITexture tex, IReadOnlyBox2D extents)
		{
			this.tex = tex;
			this.Rect = extents;
		}

		public void Draw()
		{
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(Color.White);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(Rect.MinX, Rect.MinY);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(Rect.MaxX, Rect.MinY);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(Rect.MaxX, Rect.MaxY);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(Rect.MinX, Rect.MaxY);
			GL.End();
			tex.Deactivate();
		}

		public IReadOnlyBox2D Rect { get; private set; }
		private ITexture tex;
	}
}
