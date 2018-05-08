using Zenseless.Geometry;

namespace Example
{
	internal class Node
	{
		public Box2D Boundaries;
		public Transformation3D Transformation;

		public Node(Box2D boundaries, Transformation3D transform = null)
		{
			Boundaries = boundaries;
			Transformation = transform;
		}
	}
}
