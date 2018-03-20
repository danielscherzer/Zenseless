using System.Collections.Generic;
using Zenseless.Geometry;

namespace Example
{
	public class Model
	{
		public IEnumerable<IReadOnlyCircle> ShapeBounds => new[] { playerBounds, asteroidBounds };

		public void Update(float movementXaxis, float updatePeriod)
		{
			//player movement
			playerBounds.CenterX += movementXaxis * updatePeriod;

			if (!asteroidBounds.Intersects(playerBounds))
			{
				//no intersection -> move obstacle
				asteroidBounds.CenterY -= 0.5f * updatePeriod;
			}

			if (asteroidBounds.CenterY + asteroidBounds.Radius < -1)
			{
				//asteroid has left the screen -> let him reenter from above
				asteroidBounds.CenterY = 1 + asteroidBounds.Radius;
			}
		}

		private Circle asteroidBounds = new Circle(-0.2f, 1, 0.2f);
		private Circle playerBounds = new Circle(0.0f, -0.85f, 0.1f);
	}
}
