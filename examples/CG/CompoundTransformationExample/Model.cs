namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	public class Model
	{
		public Model()
		{
			rootRotation = new Rotation3D(Axis.Z, 0f);
			var size = 0.4f;
			var moonBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);

			var delta = earth.SizeX;
			nodes.Add(new Node(moonBox, new Translation3D(-delta, -delta, 0f, rootRotation)));
			nodes.Add(new Node(moonBox, new Translation3D(-delta, delta, 0f, rootRotation)));
			nodes.Add(new Node(moonBox, new Translation3D(delta, -delta, 0f, rootRotation)));
			nodes.Add(new Node(moonBox, new Translation3D(delta, delta, 0f, rootRotation)));

			size *= 0.5f;
			delta = size + 0.05f;
			var miniBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);
			miniRotation = new Rotation3D(Axis.Z, 0f, nodes[0].Transformation);
			var miniT = new Translation3D(-delta, -delta, 0f, miniRotation);
			nodes.Add(new Node(miniBox, miniT));

			size *= 0.5f;
			delta = size;
			var mmBox = Box2DExtensions.CreateFromCenterSize(0f, 0f, size, size);
			mmRotation = new Rotation3D(Axis.Z, 0f, miniT);
			nodes.Add(new Node(mmBox, new Translation3D(-delta, -delta, 0f, mmRotation)));
		}

		public IEnumerable<IReadOnlyBox2D> GetPlanets()
		{
			var planets = new List<Box2D>();
			foreach (var node in nodes)
			{
				var transform = node.Transformation.CalcLocalToWorldRowMajorMatrix();
				var box = node.Boundaries;
				var center = box.GetCenter();
				var newCenter = Vector3.Transform(new Vector3(center, 0f), transform);
				planets.Add(Box2DExtensions.CreateFromCenterSize(newCenter.X, newCenter.Y, box.SizeX, box.SizeY));
			}
			return planets;
		}

		public Box2D Earth => earth;

		public void Update(float updatePeriod)
		{
			rootRotation.Degrees += updatePeriod * 100f;
			miniRotation.Degrees -= updatePeriod * 300f;
			mmRotation.Degrees += updatePeriod * 500f;
		}

		private Box2D earth = Box2DExtensions.CreateFromCenterSize(0f, 0f, 0.5f, 0.5f);
		private List<Node> nodes = new List<Node>();
		private readonly Rotation3D rootRotation;
		private readonly Rotation3D miniRotation;
		private readonly Rotation3D mmRotation;
	}
}