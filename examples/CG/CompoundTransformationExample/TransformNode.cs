using System.Collections.Generic;
using Zenseless.Geometry;

namespace Example
{
	public class TransformNode
	{
		public TransformNode(Transformation localTransformation, TransformNode parent)
		{
			globalTransform = new CachedCalculatedValue<Transformation>(CalcGlobalTransformation);
			LocalTransformation = localTransformation;
			Parent = parent;
		}

		public TransformNode(TransformNode parent)
		{
			globalTransform = new CachedCalculatedValue<Transformation>(CalcGlobalTransformation);
			LocalTransformation = Transformation.Identity;
			Parent = parent;
		}

		//public void AddChild(TransformNode child)
		//{
		//	if (child == null)
		//	{
		//		throw new System.ArgumentNullException(nameof(child));
		//	}

		//	children.Add(child);
		//	child.Parent = this;
		//}

		//public void RemoveChild(TransformNode child)
		//{
		//	if (child == null)
		//	{
		//		throw new System.ArgumentNullException(nameof(child));
		//	}

		//	children.Remove(child);
		//	child.Parent = null;
		//}

		public Transformation LocalTransformation
		{
			get => localTransform;
			set
			{
				localTransform = value;
				globalTransform.Invalidate();
			}
		}

		public Transformation GlobalTransformation => globalTransform.Value;

		private TransformNode Parent
		{
			get => parent;
			set
			{
				parent = value;
				globalTransform.Invalidate();
			}
		}

		//private readonly List<TransformNode> children = new List<TransformNode>();
		private CachedCalculatedValue<Transformation> globalTransform;
		private TransformNode parent;
		private Transformation localTransform;

		private Transformation CalcGlobalTransformation()
		{
			if (Parent is null) return LocalTransformation;
			return Transformation.Combine(LocalTransformation, Parent.GlobalTransformation);
		}
	}
}
