namespace Example
{
	using System.Numerics;
	using Zenseless.Geometry;

	/// <summary>
	/// Contains transformed and internally untransformed version of Box2D
	/// </summary>
	public class TransformedShape
	{
		public TransformedShape(float x, float y, float size)
		{
			this.bounds = Box2DExtensions.CreateFromCircle(new Circle(x, y, size / 2));
			transformedBounds = new Box2D(bounds);
			SetTransformation(Matrix3x2.Identity);
		}

		/// <summary>
		/// Gets or sets the transformation that will be applied to bounds to create TransformedBounds.
		/// </summary>
		/// <returns>
		/// The transformation.
		/// </returns>
		public Matrix3x2 GetTransformation()
		{
			return transformation;
		}

		/// <summary>
		/// Gets or sets the transformation that will be applied to bounds to create TransformedBounds.
		/// </summary>
		/// <param name="value">
		/// The transformation.
		/// </param>
		public void SetTransformation(Matrix3x2 value)
		{
			transformation = value;
			var newCenter = Vector2.Transform(bounds.GetCenter(), GetTransformation());
			transformedBounds.CenterX = newCenter.X;
			transformedBounds.CenterY = newCenter.Y;
		}
		public IReadOnlyBox2D TransformedBounds => transformedBounds;

		private readonly IReadOnlyBox2D bounds;
		private Box2D transformedBounds;
		private Matrix3x2 transformation;
	}
}
