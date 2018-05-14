namespace Zenseless.Geometry
{
	using System.ComponentModel;
	using System.Numerics;

	/// <summary>
	/// A generic camera class
	/// </summary>
	/// <typeparam name="VIEW">The type of the view.</typeparam>
	/// <typeparam name="PROJECTION">The type of the projection.</typeparam>
	public class Camera<VIEW, PROJECTION> : ITransformation where VIEW : INotifyPropertyChanged, ITransformation where PROJECTION : INotifyPropertyChanged, ITransformation
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
			var parent = new TransformationHierarchyNode(new Transformation(projection), null);
			Projection.PropertyChanged += (s, a) => parent.LocalTransformation = new Transformation(projection);
			node = new TransformationHierarchyNode(new Transformation(view), parent);
			view.PropertyChanged += (s, a) => node.LocalTransformation = new Transformation(view);
		}

		/// <summary>
		/// Gets the transformation matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix => node.GlobalTransformation.Matrix;

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
