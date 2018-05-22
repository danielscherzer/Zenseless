using System;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	public class Unit : IUnit
	{
		public Unit(float x, float y, float size, float orientation)
		{
			circle = new Circle(x, y, 0.5f * size);
			Orientation = orientation;
		}

		public bool IsMoving => destination.HasValue;
		public IReadOnlyCircle Bounds => circle;
		public float Orientation { get; private set; }

		public bool Selected { get; set; }

		public void HandleCollision(Unit unit)
		{
			void UndoMovement(Unit u)
			{
				u.circle.Center = u.uncollidedPos;
				//++undoCounter;
				//if (60 < undoCounter) u.destination = null;
			}

			if (Bounds.Intersects(unit.Bounds))
			{
				if (IsMoving && unit.IsMoving)
				{
					//both moving; let nearer move
					var dist1 = (destination.Value - circle.Center).Length();
					var dist2 = (unit.destination.Value - unit.circle.Center).Length();
					if (dist1 < dist2)
					{
						UndoMovement(unit);
					}
					else
					{
						UndoMovement(this);
					}
					//still collision after one reset -> reset both
					if (Bounds.Intersects(unit.Bounds))
					{
						UndoMovement(this);
						UndoMovement(unit);
					}
				}
				else if (IsMoving)
				{
					UndoMovement(this);
				}
				else if (unit.IsMoving)
				{
					UndoMovement(unit);
				}
				else
				{
					Console.WriteLine("Two none moving collide!");
				}
			}
		}

		public void MoveTo(Vector2 coord)
		{
			destination = coord;
		}

		public void Update()
		{
			var pos = circle.Center;
			uncollidedPos = pos;
			//movement required?
			if (!IsMoving) return;
			//movement direction
			var dir = destination.Value - pos;
			//movement
			var distance = dir.Length();
			//at destination?
			if (distance < 0.001f)
			{
				destination = null;
				return;
			}
			dir /= distance; // normalize
			//if rotating -> no movement
			if (UpdateOrientation(dir)) return;

			pos += dir * 0.002f;
			circle.Center = pos;
		}

		private Circle circle;

		private Vector2? destination;
		private Vector2 uncollidedPos;
		//private int undoCounter = 0;
		//private bool collided = false;

		private bool UpdateOrientation(in Vector2 dir)
		{
			var currentDirection = MathHelper.ToCartesian(new Vector2(MathHelper.DegreesToRadians(Orientation), 1f));
			var dot = Vector2.Dot(dir, currentDirection);
			if(dot < 0.99)
			{
				var det = MathHelper.Determinant(currentDirection, dir);
				var angle = Math.Atan2(det, dot);
				var ccw = Math.Sign(angle);
				Orientation += ccw;
				return true;
			}
			else
			{
				var destinationAngle = MathHelper.RadiansToDegrees(MathHelper.PolarAngle(dir));
				Orientation = destinationAngle;
				return false;
			}
		}
	}
}
