namespace Example
{
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	public class Model
	{
		public Model()
		{
			root = new Node(Transformation.Scale(0.6f), null);
			root.AddComponent(new CreateGeometryTag());

			var rootRotation = new Node(root);
			EstablishNotification(rootRotation, rotation1);

			AddGen1(rootRotation);
		}

		private void AddGen1(Node parent)
		{
			var st = Transformation.Combine(Transformation.Scale(0.6f), Transformation.Translation(1f, 0f, 0f));
			for (int i = 0; i < 4; ++i)
			{
				var xform = Transformation.Combine(st, Transformation.Rotation(90f * i));

				var node = new Node(xform, parent);
				node.AddComponent(new CreateGeometryTag());

				var nodeRotation = new Node(node);
				EstablishNotification(nodeRotation, rotation2);

				AddGen2(nodeRotation);
			}
		}

		private void AddGen2(Node parent)
		{
			var child = new Node(Transformation.Combine(Transformation.Scale(0.4f), Transformation.Translation(0.7f, 0f, 0f)), parent);
			child.AddComponent(new CreateGeometryTag());

			var childRotation = new Node(child);
			EstablishNotification(childRotation, rotation3);

			AddGen3(childRotation);
		}

		private static void AddGen3(Node parent)
		{
			var st = Transformation.Combine(Transformation.Scale(0.2f), Transformation.Translation(0.55f, 0f, 0f));
			for (int i = 0; i < 8; ++i)
			{
				var xform = Transformation.Combine(st, Transformation.Rotation(45f * i));

				var grandChild = new Node(xform, parent);
				grandChild.AddComponent(new CreateGeometryTag());
			}
		}

		private void EstablishNotification(Node node, INotifyingTransform transform)
		{
			transform.PropertyChanged += (s, a) => node.LocalTransformation = new Transformation(transform);
		}

		public IEnumerable<IReadOnlyBox2D> GetPlanets()
		{
			var planets = new List<Box2D>();
			Foreach(root, (node) => 
			{
				if (node.GetComponent<CreateGeometryTag>() is null) return;
				var xForm = node.GlobalTransformation;
				var newCenter = xForm.Transform(Vector2.Zero);
				var size = xForm.Transform(new Vector4(1f, 0f, 0f, 0f)).XY().Length();
				var bounds = Box2DExtensions.CreateFromCenterSize(newCenter.X, newCenter.Y, size, size);
				planets.Add(bounds);
			});
			return planets;
		}

		public void Update(float updatePeriod)
		{
			rotation1.Degrees += updatePeriod * 100f;
			rotation2.Degrees -= updatePeriod * 300f;
			rotation3.Degrees += updatePeriod * 500f;
		}

		private class CreateGeometryTag { };

		private Rotation rotation1 = new Rotation(0f), rotation2 = new Rotation(0f), rotation3 = new Rotation(0f);
		private readonly Node root;

		private void Foreach(Node node, Action<Node> action)
		{
			action(node);
			foreach (var child in node.Children) Foreach(child, action);
		}
	}
}