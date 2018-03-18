using Zenseless.Base;
using Zenseless.Geometry;
using System;

namespace MiniGalaxyBirds
{
	class Enemy : IComponent, ITimedUpdate
	{
		public Enemy(Component<Box2D> frame, float absoluteTime, float speedY)
		{
			this.frame = frame.Value;
			this.speedY = speedY;
			lastUpdate = absoluteTime;
			startTime = absoluteTime;
		}

		public void Update(float absoluteTime)
		{
			float timeDelta = absoluteTime - lastUpdate;
			this.frame.MinX = (float)Math.Sin((absoluteTime - startTime) * 2.0f) * 0.5f + 0.5f;
			this.frame.MinY += speedY * timeDelta;
			lastUpdate = absoluteTime;
		}

		private readonly float speedY;
		private float lastUpdate = 0.0f;
		private Box2D frame;
		private readonly float startTime;
	}
}
