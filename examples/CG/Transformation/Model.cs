namespace Example
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using Zenseless.Patterns;
	using Zenseless.Geometry;

	internal class Model
	{
		public Model()
		{
			time = new GameTime();
			//I use Matrix3x2, which is a dot net matrix class that works with row-major matrices 
			//so <new point> = <point> * <first transform> * <second transform>
			shapes.Add(new Enemy(0, 0, 0.5f));
			shapes.Add(new Enemy(-0.5f, 0f, 0.1f));
			shapes.Add(new Enemy(-0.7f, 0f, 0.03f));
		}

		internal void Update(float updatePeriod)
		{
			time.NewFrame();
			var t = time.AbsoluteTime;

			var swing = 0.8f * (float)Math.Sin(t);
			shapes[0].SetTransformation(Matrix3x2.CreateTranslation(0f, swing));

			//rotate around the center of shape[0]
			var transform = Matrix3x2.CreateRotation(2f * t) * shapes[0].GetTransformation();
			shapes[1].SetTransformation(transform);

			shapes[2].SetTransformation(transform);
		}

		private List<Enemy> shapes = new List<Enemy>();
		private GameTime time;

		public IEnumerable<IReadOnlyBox2D> Shapes { get => from s in shapes select s.TransformedBounds; }
	}
}