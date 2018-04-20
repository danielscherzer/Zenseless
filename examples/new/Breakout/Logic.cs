using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	internal class Logic
	{
		public Logic()
		{
			for(float x = -.9f; x < .9f; x += .2f)
			{
				for (float y = .2f; y < .9f; y += .2f)
				{
					bricks.Add(new Box2D(x, y, .15f, .1f));
				}
			}
		}

		public IReadOnlyBox2D Ball => ball;
		public IEnumerable<IReadOnlyBox2D> Bricks => bricks;
		public IReadOnlyBox2D Paddle => paddle;

		internal bool Update(float updatePeriod, float axisPaddle)
		{
			//move paddle
			paddle.CenterX += axisPaddle * updatePeriod * 2f;
			paddle.MinX = MathHelper.Clamp(paddle.MinX, -1, 1 - paddle.SizeX);
			//move ball
			ball.MinX += updatePeriod * ballVelocity.X;
			ball.MinY += updatePeriod * ballVelocity.Y;
			// ball vs. paddle
			if (paddle.Intersects(ball))
			{
				ballVelocity = CollisionResponse(paddle);
			}
			foreach(var brick in bricks.ToList())
			{
				if(brick.Intersects(ball))
				{
					bricks.Remove(brick);
					ballVelocity = CollisionResponse(brick);
				}
			}
			return BallVsSides();
		}

		private Vector2 CollisionResponse(IReadOnlyBox2D collider)
		{
			ball.UndoOverlap(collider);
			var newVel = ballVelocity;
			newVel.Y = -newVel.Y;
			return newVel;
		}

		private bool BallVsSides()
		{
			//ball vs. left side
			if (ball.MinX < -1)
			{
				ball.MinX = -1;
				ballVelocity.X = -ballVelocity.X;
			}
			//ball vs. right side
			else if (ball.MaxX > 1)
			{
				ball.MinX = 1 - ball.SizeX;
				ballVelocity.X = -ballVelocity.X;
			}
			//ball vs. bottom side
			else if (ball.MinY < -1)
			{
				return false;
			}
			//ball vs. top side
			else if (ball.MaxY > 1)
			{
				ball.MinY = 1 - ball.SizeY;
				ballVelocity.Y = -ballVelocity.Y;
			}
			return true;
		}

		private List<Box2D> bricks = new List<Box2D>();
		private Box2D paddle = new Box2D(-.2f, -.9f, .4f, .1f);
		private Box2D ball = new Box2D(0, 0, .1f, .1f);
		private Vector2 ballVelocity = new Vector2(0.51f, -1f);
	}
}