using OpenTK;
using System;
using System.Collections.Generic;

namespace Example
{
	public class Model
	{
		public Model()
		{
			bodies.Add(new Body(new Vector3(0, 18, 10), 50));
			bodies.Add(new Body(new Vector3(14, 0, -5), 50));
			bodies.Add(new Body(new Vector3(0, -20, 0), 5));
			bodies.Add(new Body(new Vector3(-20, 0, 5), 5));
			bodies.Add(new Body(new Vector3(11, -20, 0), 5));
			bodies.Add(new Body(new Vector3(-20, 17, 5), 5));
			//setup bodies positions and masses
			//Func<float> RndCoord = () => (Rnd01() - 0.5f) * 20.0f;
			//Func<float> R = () => (Rnd01() - 0.5f) * .01f;
			//for (int i = 0; i < 100; ++i)
			//{
			//	var body = new Body();
			//	body.Location = new Vector3(RndCoord(), RndCoord(), RndCoord());
			//	body.Velocity = new Vector3(R(), R(), R());
			//	body.Mass = Rnd01() * 20 + 0.5f;
			//	bodies.Add(body);
			//}
		}

		public IEnumerable<IBody> Bodies { get { return bodies; } }

		public void Update(float updatePeriod)
		{
			Func<float> R = () => (Rnd01() - 0.5f) * .001f;
			foreach (var b1 in bodies)
			{
				foreach (var b2 in bodies)
				{
					if (b1 == b2) continue;
					b1.ApplyForce(b1.AttractionFrom(b2));
				}
			}
			foreach (var body in bodies)
			{
				body.Update();
			}
		}

		private List<Body> bodies = new List<Body>();
		private Random random = new Random(12);

		private float Rnd01()
		{
			return (float)random.NextDouble();
		}
	}
}
