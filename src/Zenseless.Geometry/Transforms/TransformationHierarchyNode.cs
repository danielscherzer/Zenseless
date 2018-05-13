using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Transformation class that abstracts from matrices.
	/// It can return transformation matrices in row-major or column-major style.
	/// Internally it works with the row-major matrices (<seealso cref="Matrix4x4"/>).
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
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransformationHierarchyNode"/> class.
		/// </summary>
		/// <param name="transformation">The node transformation.</param>
		/// <param name="parent">The parent transformation hierarchy node.</param>
		public TransformationHierarchyNode(ITransformation transformation, TransformationHierarchyNode parent)
		{
			Transformation = transformation;
			Parent = parent;
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
		public ITransformation Transformation { get; set; }

		/// <summary>
		/// Gets a local to world transformation matrix in row-major form. 
		/// This includes the whole transformation chain with all parents
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 CalcGlobalTransformation()
		{
			if (Parent is null) return matrix;
			return matrix * Parent.CalcGlobalTransformation();
		}

		/// <summary>
		/// The matrix field for descendants
		/// </summary>
		protected Matrix4x4 matrix = Matrix4x4.Identity;
	}
}
