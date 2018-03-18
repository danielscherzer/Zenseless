using Zenseless.Base;
using Zenseless.Geometry;

namespace MiniGalaxyBirds
{
	class ConstantMovement : IComponent, ITimedUpdate
	{
		public ConstantMovement(Box2D frame, float absoluteTime, float speedX, float speedY)
		{
			this.SpeedX = speedX;
			this.SpeedY = speedY;
			this.Frame = frame;
			lastUpdate = absoluteTime;
		}
		public Box2D Frame { get; private set; }
		public float SpeedX { get; set; }
		public float SpeedY { get; set; }

		public void Update(float absoluteTime)
		{
			float timeDelta = absoluteTime - lastUpdate;
			this.Frame.MinX += SpeedX * timeDelta;
			this.Frame.MinY += SpeedY * timeDelta;
			lastUpdate = absoluteTime;
		}

		private float lastUpdate = 0.0f;
	}
}
