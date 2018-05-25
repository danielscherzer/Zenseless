namespace Example
{
	using Zenseless.Geometry;
	using System.Numerics;

	public class Collider : IBox2DCollider
	{
		public Collider(float x, float y, float sizeX, float sizeY)
		{
			Box = new Box2D(x, y, sizeX, sizeY);
			Velocity = Vector2.Zero;
		}

		public Box2D Box { get; set; }
		public Vector2 Velocity { get; set; }

		public float MinX { get	{ return Box.MinX; } }

		public float MinY { get { return Box.MinY; } }

		public float MaxX { get { return Box.MaxX; } }

		public float MaxY { get { return Box.MaxY; } }

		public void SaveBox()
		{
			savedBox = new Box2D(Box);
		}

		public void RestoreSavedBox()
		{
			if (savedBox is null) return;
			Box = savedBox;
		}

		private Box2D savedBox = null;
	}
}
