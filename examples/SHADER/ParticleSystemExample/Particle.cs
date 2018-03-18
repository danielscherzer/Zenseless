using OpenTK;

namespace Example
{
	public class Particle : IParticle
	{
		public Particle(float creationTime)
		{
			CreationTime = creationTime;
			LifeTime = float.PositiveInfinity;
		}

		public void Update(float deltaTime)
		{
			Velocity += Acceleration * deltaTime;
			Position += Velocity * deltaTime;
		}

		public Vector3 Position;
		public Vector3 Velocity;
		public Vector3 Acceleration;

		/// <summary>
		/// Controls how long a particle lives in seconds
		/// </summary>
		public float LifeTime { get; set; }
		public float CreationTime { get; private set; }
	}
}
