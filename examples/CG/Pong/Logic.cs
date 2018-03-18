using OpenTK;
using Zenseless.Geometry;

namespace Pong
{
	internal class Logic
	{
		public Logic()
		{
			ResetBall(true);
		}

		public IReadOnlyBox2D Ball => ball;
		public IReadOnlyBox2D Paddle1 => paddle1;
		public IReadOnlyBox2D Paddle2 => paddle2;
		public int Player1Points { get; private set; } = 0;
		public int Player2Points { get; private set; } = 0;

		public void Update(float updatePeriod, bool reset, float paddle1Axis, float paddle2Axis)
		{
			if (reset)
			{
				ResetBall(true);
			}

			paddle1.MinY = MovePaddle(paddle1.MinY, updatePeriod * paddle1Axis);
			paddle2.MinY = MovePaddle(paddle2.MinY, updatePeriod * paddle2Axis);
			//move ball
			ball.MinX += updatePeriod * ballV.X;
			ball.MinY += updatePeriod * ballV.Y;
			//reflect ball
			if (ball.MaxY > 1.0f || ball.MinY < -1.0)
			{
				ballV.Y = -ballV.Y;
			}
			//points
			if (ball.MinX > 1.0f)
			{
				++Player1Points;
				ResetBall(false);
			}
			if (ball.MaxX < -1.0)
			{
				++Player2Points;
				ResetBall(true);
			}
			//paddle vs ball
			if (paddle1.Intersects(ball))
			{
				ballV.Y = PaddleBallResponse(paddle1, ball);
				ballV.X = 1.0f;
			}
			if (paddle2.Intersects(ball))
			{
				ballV.Y = PaddleBallResponse(paddle2, ball);
				ballV.X = -1.0f;
			}
		}

		private Box2D paddle1 = new Box2D(-0.95f, -0.2f, 0.05f, 0.4f);
		private Box2D paddle2 = new Box2D(0.9f, -0.2f, 0.05f, 0.4f);
		private Box2D ball = new Box2D(0.0f, 0.0f, 0.1f, 0.1f);
		private Vector2 ballV = new Vector2(1.0f, 0.0f);

		private static float MovePaddle(float paddleY, float axis)
		{
			paddleY += axis;
			return OpenTK.MathHelper.Clamp(paddleY, -1.0f, 0.6f);
		}

		private static float PaddleBallResponse(IReadOnlyBox2D paddle, IReadOnlyBox2D ball)
		{
			float vY = (paddle.CenterY - ball.CenterY) / (0.5f * paddle.SizeY);
			vY = OpenTK.MathHelper.Clamp(vY, -2.0f, 2.0f);
			return vY;
		}

		private void ResetBall(bool toPlayer2)
		{
			ball.MinX = 0.0f;
			ball.MinY = 0.0f;
			ballV = new Vector2(toPlayer2 ? 1.0f : -1.0f, 0.0f);
		}
	}
}