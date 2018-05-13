namespace Zenseless.Geometry
{
	public class TransformationNode
	{
		public TransformationNode(TransformationNode parent)
		{
			Transformation = Geometry.Transformation.Identity();
			Parent = parent;
		}

		public TransformationNode(ITransformation transformation, TransformationNode parent = null)
		{
			Transformation = transformation;
			Parent = parent;
			//Parent?.children.Add(this);
		}

		public TransformationNode Parent { get; }
		public ITransformation Transformation { get; set; }

		public ITransformation CalcGlobalTransformation()
		{
			if(Parent is null) return Transformation;
			return Geometry.Transformation.Combine(Transformation, Parent.CalcGlobalTransformation());
		}

		//private readonly List<TransformationNode> children = new List<TransformationNode>();
	}
}
