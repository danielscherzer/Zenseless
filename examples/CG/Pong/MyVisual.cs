using OpenTK.Graphics.OpenGL;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Pong
{
	public class MyVisual
	{
		public MyVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			this.renderState = renderState;
			font = new TextureFont(contentLoader.Load<ITexture2D>("Big Cheese"), 10, 32);
		}

		public void Render(IReadOnlyBox2D paddle1, IReadOnlyBox2D paddle2, IReadOnlyBox2D ball, int player1Points, int player2Points)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawPaddle(paddle1);
			DrawPaddle(paddle2);
			DrawCircle(ball.CenterX, ball.CenterY, 0.5f * ball.SizeX);
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
			GL.Color4(1.0, 1.0, 1.0, 1.0);
			string score = player1Points.ToString() + '-' + player2Points.ToString();
			font.Print(-0.5f * font.Width(score, 0.1f), -0.9f, 0.0f, 0.1f, score);
			renderState.Set(BlendStates.Opaque);
		}

		private TextureFont font;
		private IRenderState renderState;

		private static void DrawCircle(float centerX, float centerY, float radius)
		{
			GL.Color3(OpenTK.Color.Red);
			DrawTools.DrawCircle(centerX, centerY, radius, 40);
		}

		private static void DrawPaddle(IReadOnlyBox2D frame)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(OpenTK.Color.Green);
			GL.Vertex2(frame.MinX, frame.MinY);
			GL.Vertex2(frame.MaxX, frame.MinY);
			GL.Vertex2(frame.MaxX, frame.MaxY);
			GL.Vertex2(frame.MinX, frame.MaxY);
			GL.End();
		}
	}
}