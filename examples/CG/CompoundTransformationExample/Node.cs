using Zenseless.Geometry;

namespace Example
{
	internal class Node
	{
		public Node(Box2D boundaries, TransformationHierarchyNode transformNode = null)
		{
			Boundaries = boundaries;
			TransformNode = transformNode;
		}

		public Box2D Boundaries { get; }
		public TransformationHierarchyNode TransformNode { get; }
	}
}
