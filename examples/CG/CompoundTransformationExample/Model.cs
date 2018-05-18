namespace Example
{
	using System.Collections.Generic;
	using Zenseless.Geometry;

	public class Model
	{
		public Model()
		{
			//rootRot = CreateChildNode(out TransformationHierarchyNode rootNode, );
			//var rootNode = new TransformationHierarchyNode(null);
			//UpdateNodeTransformation(rootNode, rootRot);

			//for (int i = 0; i < 4; ++i)
			//{
			//	var t = Transformation.Translation(0.7f, 0f, 0f);
			//	var r = Transformation.Rotation(90f * i);
			//	var xform = Transformation.Combine(t, r);
			//	var box = Box2DExtensions.CreateFromCircle(new Circle(0f, 0f, 0.2f));
			//	nodes.Add(new Node(box, new TransformationHierarchyNode(xform, rootNode)));
			//}

			//var size = 0.5f;
			//size *= 0.5f;
			//var miniTrans = CreateChildNode(out rotMini, size, nodes[0].TransformNode);

			//size *= 0.25f;
			//CreateChildNode(out rotmm, size, miniTrans);
		}

		private Rotation CreateChildNode(out TransformationHierarchyNode newChildNode, float size, TransformationHierarchyNode parent)
		{
			var rotation = new Rotation(0f);
			var node = new TransformationHierarchyNode(parent);
			rotation.PropertyChanged += (s, a) => node.LocalTransformation = new Transformation(rotation);

			newChildNode = new TransformationHierarchyNode(Transformation.Translation(size * 2f, 0f, 0f), node);

			var box = Box2DExtensions.CreateFromCircle(new Circle(0f, 0f, size));
			nodes.Add(new Node(box, newChildNode));
			return rotation;
		}

		public IEnumerable<IReadOnlyBox2D> GetPlanets()
		{
			var planets = new List<Box2D>();
			foreach (var node in nodes)
			{
				var transform = node.TransformNode.GlobalTransformation;
				var newBox = new Box2D(node.Boundaries);
				newBox.TransformCenter(transform);
				planets.Add(newBox);
			}
			return planets;
		}

		public Box2D Earth { get; } = Box2DExtensions.CreateFromCircle(new Circle(0f, 0f, 0.1f));

		public void Update(float updatePeriod)
		{
			rootRot.Degrees += updatePeriod * 100f;
			rotMini.Degrees -= updatePeriod * 300f;
			rotmm.Degrees += updatePeriod * 500f;
		}

		private List<Node> nodes = new List<Node>();
		private Rotation rootRot, rotMini, rotmm;
	}
}