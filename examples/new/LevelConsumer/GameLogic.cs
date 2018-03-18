using Zenseless.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
	public class GameLogic
	{
		public delegate void NewPositionHandler(string name, float x, float y);

		public Box2D Bounds { get; set; }
		public event NewPositionHandler NewPosition;

		public void AddCollider(string name, Circle bounds)
		{
			var collider = new Collider() { Name = name.ToLower(), Bounds = bounds };
			if ("player" == name)
			{
				player = collider;
			}
			else
			{
				colliders.Add(collider);
			}
		}

		public void Update(float updatePeriod, float axisLeftRight, float axisUpDown)
		{
			float delta = updatePeriod * 100;
			//player movement
			MovePlayer(delta * axisLeftRight, delta * axisUpDown);
		}

		struct Collider
		{
			public Circle Bounds;
			public string Name;
		}
		private List<Collider> colliders = new List<Collider>();
		private Collider player;

		private void MovePlayer(float deltaX, float deltaY)
		{
			player.Bounds.CenterX += deltaX;
			player.Bounds.CenterY += deltaY;
			var collisions = new List<Circle>();
			foreach (var collider in colliders)
			{
				if (collider.Bounds.Intersects(player.Bounds))
				{
					collisions.Add(collider.Bounds);
				}
			}
			if (1 < collisions.Count)
			{
				//more than one collision -> no movement
				player.Bounds.CenterX -= deltaX;
				player.Bounds.CenterY -= deltaY;
			}
			else if (1 == collisions.Count)
			{
				//try handling collision
				player.Bounds.UndoOverlap(collisions.First());
			}
			player.Bounds.PushXRangeInside(Bounds.MinX, Bounds.MaxX);
			player.Bounds.PushYRangeInside(Bounds.MinY, Bounds.MaxY);
			NewPosition?.Invoke(player.Name, player.Bounds.CenterX, player.Bounds.CenterY);
		}
	}
}
