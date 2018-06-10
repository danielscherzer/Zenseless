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
			root.RegisterTypeInstance(new CreateGeometryTag());

			var nodeRotation = AddRotationNode(root, 100f / 60f);
			AddGen1(nodeRotation);
		}

		private void AddGen1(Node parent)
		{
			var st = Transformation.Combine(Transformation.Scale(0.6f), Transformation.Translation(1f, 0f, 0f));
			for (int i = 0; i < 4; ++i)
			{
				var xform = Transformation.Combine(st, Transformation.Rotation(90f * i));
				var node = new Node(xform, parent);
				node.RegisterTypeInstance(new CreateGeometryTag());
				var nodeRotation = AddRotationNode(node, -300f / 60f);
				AddGen2(nodeRotation);
			}
		}

		private void AddGen2(Node parent)
		{
			var child = new Node(Transformation.Combine(Transformation.Scale(0.4f), Transformation.Translation(0.7f, 0f, 0f)), parent);
			child.RegisterTypeInstance(new CreateGeometryTag());
			var nodeRotation = AddRotationNode(child, 500f / 60f);
			AddGen3(nodeRotation);
		}

		private Node AddRotationNode(Node parent, float degreeIncrement)
		{
			var node = new Node(parent);
			var rotation = new Rotation(0f);
			eachFrameCallbacks.Add(() => rotation.Degrees += degreeIncrement);
			rotation.PropertyChanged += (s, a) => node.LocalTransformation = new Transformation(rotation);
			return node;
		}

		private static void AddGen3(Node parent)
		{
			var st = Transformation.Combine(Transformation.Scale(0.2f), Transformation.Translation(0.55f, 0f, 0f));
			for (int i = 0; i < 8; ++i)
			{
				var xform = Transformation.Combine(st, Transformation.Rotation(45f * i));

				var grandChild = new Node(xform, parent);
				grandChild.RegisterTypeInstance(new CreateGeometryTag());
			}
		}

		public IEnumerable<IReadOnlyBox2D> GetPlanets()
		{
			var planets = new List<Box2D>();
			Foreach(root, (node) =>
			{
				if (node.GetInstance<CreateGeometryTag>() is null) return;
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
			foreach (var action in eachFrameCallbacks)
			{
				action();
			}
		}

		private class CreateGeometryTag { };

		private readonly Node root;
		private List<Action> eachFrameCallbacks = new List<Action>();

		private void Foreach(Node node, Action<Node> action)
		{
			action(node);
			foreach (var child in node.Children) Foreach(child, action);
		}
	}
}