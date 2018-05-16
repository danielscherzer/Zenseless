using System;
using System.Collections.Generic;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	public class Model
	{
		/// <summary>
		/// Locations of all particles
		/// </summary>
		public IEnumerable<IParticle> Particles => particles;

		/// <summary>
		/// Particle source (emitter)
		/// </summary>
		public Vector2 Emitter { get; set; }

		public Model(int elementCount)
		{
			//set random location and velocity for each point, to have a nice simulation start
			for (int i = 0; i < elementCount; ++i)
			{
				//the concept of particle age is also used in "Framework\SHADER\Examples\ParticleSystemExample\ParticleSystem.cs"
				var particle = new Particle();
				Seed(particle);
				particle.Age = Rnd01(); //start with random ages to have enough randomness
				particles.Add(particle);
			}
			//run the simulation for a few steps to have a realistic starting point to display
			for(int i = 0; i < 100; ++i) Update(.1f);
		}

		public void Update(float deltaTime)
		{
			foreach (var particle in particles)
			{
				particle.ApplyForce(new Vector2(.3f, -.3f)); //wind: upward draft, slightly to the right
				particle.Update(deltaTime);
				var lifeTime = 5f;  //particles life 5 seconds
				particle.Age += deltaTime / lifeTime;
				
				if (!particle.IsAlive)
				{
					Seed(particle);
				}
			}
		}

		//need random numbers for particle seeding
		private Random rnd = new Random(12);
		private List<Particle> particles = new List<Particle>();
		private readonly IReadOnlyBox2D screen = new Box2D(-1, -1, 2, 2);

		//helper random function
		private float Rnd01() => (float)rnd.NextDouble();
		private float RndM11() => (Rnd01() - 0.5f) * 2.0f;

		private void Seed(Particle particle)
		{
			var velocity = new Vector2(RndM11() * .1f, Rnd01()) * 0.1f; //moving mainly upward
			particle.Seed(Emitter, velocity);
		}
	}
}
