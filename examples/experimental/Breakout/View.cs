using OpenTK.Graphics.OpenGL;
using Zenseless.Geometry;
using Zenseless.OpenGL;

namespace Example
{
	internal class View
	{
		internal void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		internal void DrawBox(IReadOnlyBox2D paddle)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(paddle.MinX, paddle.MinY);
			GL.Vertex2(paddle.MaxX, paddle.MinY);
			GL.Vertex2(paddle.MaxX, paddle.MaxY);
			GL.Vertex2(paddle.MinX, paddle.MaxY);
			GL.End();
		}

		internal void DrawBall(IReadOnlyBox2D ball)
		{
			DrawTools.DrawCircle(ball.CenterX, ball.CenterY, .05f, 20);
		}
	}
}