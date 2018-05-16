namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	public class Model
	{
		public Model()
		{
			rootRot = new Rotation(0f);
			var rootNode = new TransformationHierarchyNode(null);
			UpdateNodeTransformation(rootNode, rootRot);

			var delta = 1.1f * Earth.SizeX;
			for (int i = 0; i < 4; ++i)
			{
				var t = Transformation.Translation(delta, 0f, 0f);
				var r = Transformation.Rotation(90f * i);
				var s = Transformation.Scale(0.12f);
				var xform = Transformation.Combine(s, Transformation.Combine(r, t));
				nodes.Add(new Node(Earth, new TransformationHierarchyNode(s, rootNode)));
			}

			var size = 0.125f;
			var miniBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);

			rotMini = new Rotation(0f);
			var mini = new TransformationHierarchyNode(nodes[0].TransformNode);
			UpdateNodeTransformation(mini, rotMini);
			var miniTrans = new TransformationHierarchyNode(Transformation.Translation(2f * size , 0f, 0f), mini);
			//nodes.Add(new Node(miniBox, miniTrans));

			size *= 0.5f;
			var mmBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);
			rotmm = new Rotation(0f);
			var mm = new TransformationHierarchyNode(miniTrans);
			UpdateNodeTransformation(mm, rotmm);
			//nodes.Add(new Node(mmBox, new TransformationHierarchyNode(Transformation.Translation(size * 1.4f, 0f, 0f), mm)));
		}

		private void UpdateNodeTransformation(TransformationHierarchyNode node, Rotation rotation)
		{
			rotation.PropertyChanged += (s, a) => node.LocalTransformation = new Transformation(rotation.Matrix);
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

		public Box2D Earth { get; } = Box2DExtensions.CreateFromCircle(new Circle(0f, 0f, 0.5f));

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