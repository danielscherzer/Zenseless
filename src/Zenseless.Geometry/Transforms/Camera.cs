namespace Zenseless.Geometry
{
	using System.Numerics;

	/// <summary>
	/// A generic camera class
	/// </summary>
	/// <typeparam name="VIEW">The type of the view.</typeparam>
	/// <typeparam name="PROJECTION">The type of the projection.</typeparam>
	public class Camera<VIEW, PROJECTION> : ITransformation where VIEW : ITransformation where PROJECTION : ITransformation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Camera{VIEW, PROJECTION}"/> class.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="projection">The projection.</param>
		public Camera(VIEW view, PROJECTION projection)
		{
			View = view;
			Projection = projection;
			node = new TransformationHierarchyNode(view, new TransformationHierarchyNode(projection, null));
		}

		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix => node.CalcGlobalTransformation().Matrix;

		/// <summary>
		/// Gets the projection transformation.
		/// </summary>
		/// <value>
		/// The projection.
		/// </value>
		public PROJECTION Projection { get; }

		/// <summary>
		/// Gets the view transformation.
		/// </summary>
		/// <value>
		/// The view.
		/// </value>
		public VIEW View { get; }

		private readonly TransformationHierarchyNode node;
	}
}
