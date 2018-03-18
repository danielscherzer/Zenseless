using System.Numerics;

namespace Example
{
	public class Element : IElement
	{
		public Vector2 Coord { get; set; }
		public float Size { get; set; }
		public Vector2 Velocity { get; set; } = Vector2.Zero;

		public Element(Vector2 coord, float size)
		{
			Coord = coord;
			Size = size;
		}
	}
}
