using System.Collections.Generic;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Transformation class that supports hierarchical transformations via parent relationships.
	/// </summary>
	public class TransformationHierarchyNode
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TransformationHierarchyNode"/> class.
		/// </summary>
		/// <param name="parent">The parent transformation hierarchy node.</param>
		public TransformationHierarchyNode(TransformationHierarchyNode parent)
		{
			globalTransform = new CachedCalculatedValue<Transformation>(CalcGlobalTransformation);
			LocalTransformation = Transformation.Identity;
			Parent = parent;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransformationHierarchyNode"/> class.
		/// </summary>
		/// <param name="localTransformation">The node transformation.</param>
		/// <param name="parent">The parent transformation hierarchy node.</param>
		public TransformationHierarchyNode(Transformation localTransformation, TransformationHierarchyNode parent)
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
		public IReadOnlyList<TransformationHierarchyNode> Children => children;

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
		public TransformationHierarchyNode Parent
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

		private readonly List<TransformationHierarchyNode> children = new List<TransformationHierarchyNode>();
		private CachedCalculatedValue<Transformation> globalTransform;
		private TransformationHierarchyNode parent;
		private Transformation localTransform;

		private Transformation CalcGlobalTransformation()
		{
			if (Parent is null) return LocalTransformation;
			return Transformation.Combine(LocalTransformation, Parent.GlobalTransformation);
		}
	}
}
