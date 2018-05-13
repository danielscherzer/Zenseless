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
			Parent = parent;
			LocalTransformation = Transformation.Identity;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransformationHierarchyNode"/> class.
		/// </summary>
		/// <param name="localTransformation">The node transformation.</param>
		/// <param name="parent">The parent transformation hierarchy node.</param>
		public TransformationHierarchyNode(ITransformation localTransformation, TransformationHierarchyNode parent)
		{
			Parent = parent;
			LocalTransformation = localTransformation ?? throw new System.ArgumentNullException(nameof(localTransformation));
		}

		/// <summary>
		/// Gets the parent transformation hierarchy node.
		/// </summary>
		/// <value>
		/// The parent transformation.
		/// </value>
		public TransformationHierarchyNode Parent { get; }

		/// <summary>
		/// Gets or sets the transformation.
		/// </summary>
		/// <value>
		/// The transformation.
		/// </value>
		public ITransformation LocalTransformation { get; set; }

		/// <summary>
		/// Gets a local to world transformation. 
		/// This includes the whole transformation chain with all parents
		/// </summary>
		/// <returns></returns>
		public Transformation CalcGlobalTransformation()
		{
			if (Parent is null) return new Transformation(LocalTransformation.Matrix);
			debugCounter++;
			return Transformation.Combine(LocalTransformation, Parent.CalcGlobalTransformation());
		}

		public static int debugCounter = 0;
	}
}
