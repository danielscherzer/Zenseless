namespace Example
{
	using System.Numerics;
	using Zenseless.Geometry;

	/// <summary>
	/// Contains transformed and internally untransformed version of Box2D
	/// </summary>
	public class TransformedShape
	{
		public TransformedShape(Matrix3x2 transformation, IReadOnlyBox2D bounds)
		{
			this.bounds = bounds;
			transformedBounds = new Box2D(bounds);
			Transformation = transformation;
		}

		/// <summary>
		/// Gets or sets the transformation that will be applied to bounds to create TransformedBounds.
		/// </summary>
		/// <value>
		/// The transformation.
		/// </value>
		public Matrix3x2 Transformation
		{
			get => transformation;
			set
			{
				transformation = value;
				var newCenter = Vector2.Transform(bounds.GetCenter(), Transformation);
				transformedBounds.CenterX = newCenter.X;
				transformedBounds.CenterY = newCenter.Y;
			}
		}
		public IReadOnlyBox2D TransformedBounds => transformedBounds;

		private readonly IReadOnlyBox2D bounds;
		private Box2D transformedBounds;
		private Matrix3x2 transformation;
	}
}
