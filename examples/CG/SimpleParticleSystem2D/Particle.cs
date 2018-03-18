using System.Numerics;

namespace Example
{
	public class Particle : IParticle
	{
		/// <summary>
		/// Birth = 0; Death = 1
		/// </summary>
		public float Age { get; set; } = 0f;
		public bool IsAlive => Age < 1;
		public float Mass { get; set; } = 1f;
		public Vector2 Acceleration { get; set; } = Vector2.Zero;
		public Vector2 Location { get; set; } = Vector2.Zero;
		public Vector2 Velocity { get; set; } = Vector2.Zero;

		public void ApplyForce(Vector2 force)
		{
			//Newtons 2nd Law: Force = Mass * Acceleration; 
			//acceleration equals the sum of all forces / Mass
			Acceleration += force / Mass;
		}

		public void Seed(Vector2 location, Vector2 velocity)
		{
			Age = 0f;
			Location = location;
			Velocity = velocity;
			Acceleration = Vector2.Zero;
		}

		public void Update(float deltaTime)
		{
			//Newtons 1st law
			Velocity += Acceleration * deltaTime;
			Location += Velocity * deltaTime;
			//force was spend reset Acceleration
			Acceleration = Vector2.Zero;
		}
	}
}
