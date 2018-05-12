namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	public class Model
	{
		public Model()
		{
			root = new TransformationNode();
			rootRotation = new Rotation3D(Axis.Z, 0f);
			var size = 0.4f;
			var moonBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);

			var delta = Earth.SizeX;
			nodes.Add(new Node(moonBox, new Translation3D(-delta, -delta, 0f, rootRotation), new TransformationNode(Transformation.Translation(-delta, -delta, 0f), root)));
			nodes.Add(new Node(moonBox, new Translation3D(-delta, delta, 0f, rootRotation), new TransformationNode(Transformation.Translation(-delta, delta, 0f), root)));
			nodes.Add(new Node(moonBox, new Translation3D(delta, -delta, 0f, rootRotation), new TransformationNode(Transformation.Translation(delta, -delta, 0f), root)));
			nodes.Add(new Node(moonBox, new Translation3D(delta, delta, 0f, rootRotation), new TransformationNode(Transformation.Translation(delta, delta, 0f), root)));

			size *= 0.5f;
			delta = size + 0.05f;
			var miniBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);
			miniRotation = new Rotation3D(Axis.Z, 0f, nodes[0].Transformation);
			var miniT = new Translation3D(-delta, -delta, 0f, miniRotation);

			mini = new TransformationNode(nodes[0].TransformNode);
			var miniTrans = new TransformationNode(Transformation.Translation(-delta, -delta, 0f), mini);
			nodes.Add(new Node(miniBox, miniT, miniTrans));

			size *= 0.5f;
			delta = size;
			var mmBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);
			mmRotation = new Rotation3D(Axis.Z, 0f, miniT);
			mm = new TransformationNode(miniTrans);
			nodes.Add(new Node(mmBox, new Translation3D(-delta, -delta, 0f, mmRotation), new TransformationNode(Transformation.Translation(-delta, -delta, 0f), mm)));
		}

		public IEnumerable<IReadOnlyBox2D> GetPlanets()
		{
			var planets = new List<Box2D>();
			foreach (var node in nodes)
			{
				//var transform = node.Transformation.CalcLocalToWorldRowMajorMatrix();
				var transform = node.TransformNode.CalcGlobalTransformation().GetLocalRowMajorMatrix();
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
			rootRot += updatePeriod * 100f;
			root.Transformation = Transformation.Rotation(Axis.Z, rootRot);

			miniRot -= updatePeriod * 300f;
			mini.Transformation = Transformation.Rotation(Axis.Z, miniRot);

			mmRot += updatePeriod * 500f;
			mm.Transformation = Transformation.Rotation(Axis.Z, mmRot);

			rootRotation.Degrees += updatePeriod * 100f;
			miniRotation.Degrees -= updatePeriod * 300f;
			mmRotation.Degrees += updatePeriod * 500f;
		}

		private List<Node> nodes = new List<Node>();
		private readonly TransformationNode root, mini, mm;
		private readonly Rotation3D rootRotation;
		private float rootRot = 0f, miniRot = 0f, mmRot = 0f;
		private readonly Rotation3D miniRotation;
		private readonly Rotation3D mmRotation;
	}
}