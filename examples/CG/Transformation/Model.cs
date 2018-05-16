namespace Example
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using Zenseless.Base;
	using Zenseless.Geometry;

	internal class Model
	{
		public Model()
		{
			time = new GameTime();
			shapes.Add(new TransformedShape(Matrix3x2.Identity, Box2DExtensions.CreateFromCircle(new Circle(0f, 0f, 0.25f))));
			shapes.Add(new TransformedShape(Matrix3x2.CreateTranslation(0f, 0f), Box2DExtensions.CreateFromCircle(new Circle(-0.5f, 0f, 0.1f))));
		}

		internal void Update(float updatePeriod)
		{
			time.NewFrame();
			var t = time.AbsoluteTime;

			var swing = 0.8f * (float)Math.Sin(t);
			shapes[0].Transformation = Matrix3x2.CreateTranslation(swing, 0f);

			//rotate along an ellipsoid around the center of shape[0]
			var transform = Matrix3x2.CreateRotation(2f * t) * Matrix3x2.CreateScale(1f, 2f) * shapes[0].Transformation;
			shapes[1].Transformation = transform;
		}

		private List<TransformedShape> shapes = new List<TransformedShape>();
		private GameTime time;

		public IEnumerable<IReadOnlyBox2D> Shapes { get => from s in shapes select s.TransformedBounds; }
	}
}