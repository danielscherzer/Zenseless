namespace Zenseless.Geometry
{
	using System.ComponentModel;
	using System.Numerics;
	using Zenseless.Patterns;

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
			cachedMatrix = new CachedCalculatedValue<Matrix4x4>(CalcCombinedTransform);

			View.PropertyChanged += (s, a) => cachedMatrix.Invalidate();
			Projection.PropertyChanged += (s, a) => cachedMatrix.Invalidate();
		}

		/// <summary>
		/// Gets the model-view-projection matrix in row-major style.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public Matrix4x4 Matrix => cachedMatrix.Value;

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

		private CachedCalculatedValue<Matrix4x4> cachedMatrix;

		private Matrix4x4 CalcCombinedTransform()
		{
			return View.Matrix * Projection.Matrix;
		}
	}
}
