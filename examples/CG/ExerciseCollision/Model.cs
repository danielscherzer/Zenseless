using System.Collections.Generic;
using Zenseless.Geometry;

namespace Example
{
	public class Model
	{
		public IEnumerable<IReadOnlyCircle> Shapes => new[] { player, obstacle };

		public void Update(float movementXaxis, float updatePeriod)
		{
			//player movement
			player.CenterX += movementXaxis * updatePeriod;

			//TODO student: let the player also move up and down
			//TODO student: Limit player movements to window

			//no intersection -> move obstacle
			if (!obstacle.Intersects(player))
			{
				obstacle.CenterY -= 0.5f * updatePeriod;
			}

			if (obstacle.CenterY + obstacle.Radius < -1)
			{
				obstacle.CenterY = 1 + obstacle.Radius;
			}
		}

		private Circle obstacle = new Circle(-0.2f, 1, 0.2f);
		private Circle player = new Circle(0.0f, -0.85f, 0.1f);
	}
}
