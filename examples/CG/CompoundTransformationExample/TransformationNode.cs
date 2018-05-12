namespace Zenseless.Geometry
{
	public class TransformationNode
	{
		public TransformationNode(TransformationNode parent = null)
		{
			Transformation = Transformation.Identity();
			Parent = parent;
		}

		public TransformationNode(Transformation transformation, TransformationNode parent = null)
		{
			Transformation = transformation;
			Parent = parent;
			//Parent?.children.Add(this);
		}

		public TransformationNode Parent { get; }
		public Transformation Transformation { get; set; }

		public Transformation CalcGlobalTransformation()
		{
			if(Parent is null) return Transformation;
			return Transformation.Combine(Transformation, Parent.CalcGlobalTransformation());
		}

		//private readonly List<TransformationNode> children = new List<TransformationNode>();
	}
}
