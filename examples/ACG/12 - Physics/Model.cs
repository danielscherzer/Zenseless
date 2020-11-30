using System.Collections.Generic;
using System.Numerics;

namespace Example
{
	public class Model
	{
		public Model()
		{
			//for each body - setup body position and mass
			bodies.Add(new Body(new Vector3(0, 18, 10), 50));
			bodies.Add(new Body(new Vector3(14, 0, -5), 50));
			bodies.Add(new Body(new Vector3(0, -20, 0), 5));
			bodies.Add(new Body(new Vector3(-20, 0, 5), 5));
			bodies.Add(new Body(new Vector3(11, -20, 0), 5));
			bodies.Add(new Body(new Vector3(-20, 17, 5), 5));
		}

		public IEnumerable<IBody> Bodies { get { return bodies; } }

		public void Update(float updatePeriod)
		{
#if SOLUTION
			foreach (var b1 in bodies)
			{
				foreach (var b2 in bodies)
				{
					if (b1 == b2) continue;
					b1.ApplyForce(b1.AttractionFrom(b2));
				}
			}
#endif
			foreach (var body in bodies)
			{
				body.Update();
			}
		}

		private List<Body> bodies = new List<Body>();
	}
}
