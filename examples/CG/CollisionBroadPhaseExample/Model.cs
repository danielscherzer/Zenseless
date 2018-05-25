namespace Example
{
	using System.Collections.Generic;
	using Zenseless.Geometry;

	internal class Model
	{
		public Model()
		{
			SetupColliders();
		}

		public IEnumerable<Collider> Colliders => colliders;

		public int CollisionCount { get; private set; }

		public void UpdateMovements(float updatePeriod)
		{
			//movement
			foreach (var collider in colliders)
			{
				collider.SaveBox();
				collider.Box.MinX += collider.Velocity.X * updatePeriod;
				collider.Box.MinY += collider.Velocity.Y * updatePeriod;
				if (!windowBorders.Contains(collider.Box))
				{
					collider.Box.PushXRangeInside(windowBorders);
					collider.Box.PushYRangeInside(windowBorders);
					collider.Velocity = -collider.Velocity;
				}
			}
			CollisionCount = 0;
		}

		internal void BruteForceCollision()
		{
			for (int i = 0; i < colliders.Count; ++i)
			{
				for (int j = i + 1; j < colliders.Count; ++j)
				{
					HandleNarrowPhaseCollision(colliders[i], colliders[j]);
				}
			}
		}

		internal void GridCollision()
		{
			collisionGrid.FindAllCollisions(colliders, (c1, c2) => HandleNarrowPhaseCollision(c1 as Collider, c2 as Collider));
		}

		internal void GridCollisionCenter()
		{
			collisionGridCenter.FindAllCollisions(colliders, (c1, c2) => HandleNarrowPhaseCollision(c1 as Collider, c2 as Collider));
		}

		private List<Collider> colliders = new List<Collider>();
		private IReadOnlyBox2D windowBorders = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private CollisionGrid collisionGrid;
		private CollisionGridCenter collisionGridCenter;

		private void SetupColliders()
		{
			float delta = 0.03f;
			float space = 0.01f;
			float distance2 = space / 2;
			float size = delta - space;
			int i = 0;
			for (float x = -0.9f; x < 0.9f; x += delta)
			{
				for (float y = -0.9f; y < 0.9f; y += delta)
				{
					var collider = new Collider(x, y, size, size)
					{
						Velocity = RandomVectors.Velocity()
					};
					colliders.Add(collider);
					++i;
				}
			}
			float scale = 2f;
			collisionGrid = new CollisionGrid(windowBorders, size * scale, size * scale);
			collisionGridCenter = new CollisionGridCenter(windowBorders, size * scale, size * scale);
		}

		private void HandleNarrowPhaseCollision(Collider collider1, Collider collider2)
		{
			var box1 = collider1.Box;
			var box2 = collider2.Box;
			if (box1.Intersects(box2))
			{
				//undo movement
				collider1.RestoreSavedBox();
				collider2.RestoreSavedBox();
				////set random velocity
				collider1.Velocity = RandomVectors.Velocity();
				collider2.Velocity = RandomVectors.Velocity();
				++CollisionCount;
			}
		}
	}
}