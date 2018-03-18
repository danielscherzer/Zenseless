using Zenseless.Geometry;
using OpenTK;

namespace Example
{
	public class Liquid
	{
		public Box3D Bounds { get; set; }

		public float DragCoefficient { get; set; }

		public Vector3 Drag(Body b)
		{
			var p = b.Location;
			if (!Bounds.Contains(p.X, p.Y, p.Z))
			{
				return Vector3.Zero;
			}
			float speed = b.Velocity.Length;
			if (0.01f > speed) return Vector3.Zero;
			float dragMagnitude = DragCoefficient * speed * speed;
			var drag = -b.Velocity;
			drag /= speed;
			drag *= dragMagnitude;
			return drag;
		}
	}
}
