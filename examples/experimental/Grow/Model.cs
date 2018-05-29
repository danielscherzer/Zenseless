using System;
using System.Collections.Generic;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	public class Model
	{
		public IEnumerable<IElement> Elements => elements;
		public IElement Player => player;

		public Model(int elementCount)
		{
			screen = new Box2D(-1, -1, 2, 2);
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;

			for (int i = 0; i < elementCount; ++i)
			{
				var element = new Element(new Vector2(RndCoord(), RndCoord()), Rnd01());
				element.Velocity = new Vector2(RndCoord(), RndCoord()) * .005f;
				elements.Add(element);
			}
		}

		public void Update(float time)
		{
			foreach (var element in elements)
			{
				element.Coord += element.Velocity;
				if (!screen.Contains(element.Coord.X, element.Coord.Y))
				{
					element.Velocity = -element.Velocity;
				}
			}
		}

		private List<Element> elements = new List<Element>();
		private Element player = new Element(Vector2.Zero, .5f);
		private readonly IReadOnlyBox2D screen;
	}
}
