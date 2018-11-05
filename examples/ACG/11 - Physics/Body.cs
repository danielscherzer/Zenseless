namespace Example
{
	using System.Diagnostics;
	using System.Numerics;
	using Zenseless.Geometry;

	/// <summary>
	/// Adapted from http://natureofcode.com/book/chapter-2-forces/
	/// </summary>
	public class Body : IBody
	{
		public Body()
		{
			Acceleration = Vector3.Zero;
			Location = Vector3.Zero;
			Mass = 1;
			Velocity = Vector3.Zero;
		}

		public Body(in Vector3 location, float mass)
		{
			Acceleration = Vector3.Zero;
			Location = location;
			Mass = mass;
			Velocity = Vector3.Zero;
		}

		public const float G = .1f; //physically correct would be 6.67408e-11f;

		public Vector3 Acceleration { get; set; }
		public Vector3 Location { get; set; }
		public float Mass { get; set; }
		public Vector3 Velocity { get; set; }

		public void ApplyForce(in Vector3 force)
		{
			//Newtons 2nd Law: Force = Mass * Acceleration; 
			//but also consider force accumulation:
			//acceleration equals the sum of all forces / Mass
			Acceleration += force / Mass;
		}

#if SOLUTION
		public Vector3 AttractionFrom(Body bodyB)
		{	
			//gravitation
			var dir = bodyB.Location - Location;
			var distance = MathHelper.Clamp(dir.Length(), 5f, 20f);
			var strength = (G * Mass * bodyB.Mass) / (distance * distance);

			return strength * Vector3.Normalize(dir);
		}
#endif
		public void Update()
		{
			//Newtons 1st law
			Velocity += Acceleration;
			Location += Velocity;
			//force was spend reset Acceleration
			Acceleration = Vector3.Zero;
		}
	}
}
