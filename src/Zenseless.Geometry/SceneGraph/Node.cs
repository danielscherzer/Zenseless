namespace Zenseless.Geometry
{
	using System.Collections.Generic;
	using Zenseless.Patterns;

	/// <summary>
	/// A scene-graph node that supports hierarchical transformations via parent relationships.
	/// </summary>
	public class Node : TypeRegistry
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Node"/> class.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		public Node(Node parent)
		{
			globalTransform = new CachedCalculatedValue<Transformation>(CalcGlobalTransformation);
			LocalTransformation = Transformation.Identity;
			Parent = parent;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Node"/> class.
		/// </summary>
		/// <param name="localTransformation">The node transformation.</param>
		/// <param name="parent">The parent node.</param>
		public Node(Transformation localTransformation, Node parent)
		{
			globalTransform = new CachedCalculatedValue<Transformation>(CalcGlobalTransformation);
			LocalTransformation = localTransformation;
			Parent = parent;
		}

		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <value>
		/// The children.
		/// </value>
		public IReadOnlyList<Node> Children => children;

		/// <summary>
		/// Invalidates this instance and its children.
		/// </summary>
		public void Invalidate()
		{
			globalTransform.Invalidate();
			foreach (var child in children) child.Invalidate();
		}

		/// <summary>
		/// Gets or sets the transformation.
		/// </summary>
		/// <value>
		/// The transformation.
		/// </value>
		public Transformation LocalTransformation
		{
			get => localTransform;
			set
			{
				localTransform = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Gets the global transformation (local to world transformation). 
		/// This includes the whole transformation chain with all parents.
		/// </summary>
		/// <value>
		/// The global transformation.
		/// </value>
		public Transformation GlobalTransformation => globalTransform.Value;

		/// <summary>
		/// Gets or sets the parent transformation hierarchy node.
		/// </summary>
		/// <value>
		/// The parent transformation.
		/// </value>
		public Node Parent
		{
			get => parent;
			set
			{
				if (!(parent is null))
				{
					parent.children.Remove(this);
				}
				if (!(value is null))
				{
					value.children.Add(this);
				}
				parent = value;
				Invalidate();
			}
		}

		private readonly List<Node> children = new List<Node>();
		private CachedCalculatedValue<Transformation> globalTransform;
		private Node parent;
		private Transformation localTransform;

		private Transformation CalcGlobalTransformation()
		{
			if (Parent is null) return LocalTransformation;
			return Transformation.Combine(LocalTransformation, Parent.GlobalTransformation);
		}
	}
}
