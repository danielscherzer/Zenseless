namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	public class Model
	{
		public Model()
		{
			rotRoot = new Rotation(0f);
			var root = new TransformationNode(rotRoot);
			var size = 0.4f;
			var moonBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);

			var delta = Earth.SizeX;
			nodes.Add(new Node(moonBox, new TransformationNode(Transformation.Translation(-delta, -delta, 0f), root)));
			nodes.Add(new Node(moonBox, new TransformationNode(Transformation.Translation(-delta, delta, 0f), root)));
			nodes.Add(new Node(moonBox, new TransformationNode(Transformation.Translation(delta, -delta, 0f), root)));
			nodes.Add(new Node(moonBox, new TransformationNode(Transformation.Translation(delta, delta, 0f), root)));

			size *= 0.5f;
			delta = size + 0.05f;
			var miniBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);

			rotMini = new Rotation(0f);
			var miniTrans = new TransformationNode(Transformation.Translation(-delta, -delta, 0f), new TransformationNode(rotMini, nodes[0].TransformNode));
			nodes.Add(new Node(miniBox, miniTrans));

			size *= 0.5f;
			delta = size;
			var mmBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);
			rotmm = new Rotation(0f);
			nodes.Add(new Node(mmBox, new TransformationNode(Transformation.Translation(-delta, -delta, 0f), new TransformationNode(rotmm, miniTrans))));
		}

		public IEnumerable<IReadOnlyBox2D> GetPlanets()
		{
			var planets = new List<Box2D>();
			foreach (var node in nodes)
			{
				var transform = node.TransformNode.CalcGlobalTransformation().Matrix;
				var box = node.Boundaries;
				var center = box.GetCenter();
				var newCenter = Vector3.Transform(new Vector3(center, 0f), transform);
				planets.Add(Box2DExtensions.CreateFromCenterSize(newCenter.X, newCenter.Y, box.SizeX, box.SizeY));
			}
			return planets;
		}

		public Box2D Earth { get; } = Box2DExtensions.CreateFromCenterSize(0f, 0f, 0.5f, 0.5f);

		public void Update(float updatePeriod)
		{
			rotRoot.Degrees += updatePeriod * 100f;
			rotMini.Degrees -= updatePeriod * 300f;
			rotmm.Degrees += updatePeriod * 500f;
		}

		private List<Node> nodes = new List<Node>();
		private Rotation rotRoot, rotMini, rotmm;
	}
}