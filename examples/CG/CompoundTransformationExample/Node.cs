using Zenseless.Geometry;

namespace Example
{
	internal class Node
	{
		public Node(Box2D boundaries, Transformation3D transform = null, TransformationNode transformNode = null)
		{
			Boundaries = boundaries;
			Transformation = transform;
			TransformNode = transformNode;
		}

		public Box2D Boundaries { get; }
		public Transformation3D Transformation { get; }
		public TransformationNode TransformNode { get; }
	}
}
