using Zenseless.Geometry;
using OpenTK;
using System.Drawing;

namespace MvcSokoban
{
	public class Drawable
	{
		public Box2D Rect { get; private set; }

		public Drawable(Point position, float speed, float size)
		{
			this.position = position;
			this.Rect = new Box2D(position.X, position.Y, size, size);
			this.speed = speed;
		}

		public Point Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
				//convert from logic to screen
				destination = new Vector2((float)value.X, (float)value.Y);
				//Console.WriteLine(destination);
			}
		}

		public void Update(float updatePeriod)
		{
			if (!ReferenceEquals(null,  destination))
			{
				Vector2 pos = new Vector2(Rect.MinX, Rect.MinY);
				Vector2 dir = destination.Value - pos;
				float length = dir.Length;
				if (length < 0.1f)
				{
					Rect.MinX = destination.Value.X;
					Rect.MinY = destination.Value.Y;
					destination = null;
					return;
				}
				dir /= length;
				dir *= speed * updatePeriod;
				pos += dir;
				Rect.MinX = pos.X;
				Rect.MinY = pos.Y;
			}
		}

		private Vector2? destination = null;
		private Point position;
		private readonly float speed;
	}
}
