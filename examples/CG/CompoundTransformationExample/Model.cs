using System.Collections.Generic;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	public class Model
	{
		private Vector2 rotCenter = new Vector2(-.9f, 0);
		private List<Box2D> birds = new List<Box2D>();

		public Model()
		{
			//generate birds
			for (float delta = .1f; delta < .5f; delta += .1f)
			{
				birds.Add(Box2DExtensions.CreateFromCenterSize(rotCenter.X - delta, rotCenter.Y - delta, .1f, .1f));
				birds.Add(Box2DExtensions.CreateFromCenterSize(rotCenter.X - delta, rotCenter.Y + delta, .1f, .1f));
				birds.Add(Box2DExtensions.CreateFromCenterSize(rotCenter.X + delta, rotCenter.Y - delta, .1f, .1f));
				birds.Add(Box2DExtensions.CreateFromCenterSize(rotCenter.X + delta, rotCenter.Y + delta, .1f, .1f));
			}
		}

		public IEnumerable<IReadOnlyBox2D> Birds => birds;

		public void Update(float updatePeriod)
		{
			rotCenter.X += updatePeriod * 0.1f;
			var t = Transformation2D.CreateRotationAround(rotCenter, updatePeriod * 200f);
			foreach (var bird in birds)
			{
				bird.TransformCenter(t);
			}
		}
	}
}