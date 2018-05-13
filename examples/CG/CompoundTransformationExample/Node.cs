using Zenseless.Geometry;

namespace Example
{
	internal class Node
	{
		public Node(Box2D boundaries, TransformationNode transformNode = null)
		{
			Boundaries = boundaries;
			TransformNode = transformNode;
		}

		public Box2D Boundaries { get; }
		public TransformationNode TransformNode { get; }
	}
}
