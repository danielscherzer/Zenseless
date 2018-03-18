using Zenseless.Geometry;

namespace MiniGalaxyBirds
{
	public class Collidable : IComponent, ICollidable
	{
		public Collidable(Box2D frame)
		{
			this.Frame = frame;
		}

		public Box2D Frame { get; private set; }
	}
}
