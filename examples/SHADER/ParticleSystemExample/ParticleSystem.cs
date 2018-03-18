using System;
using System.Collections.Generic;

namespace Example
{
	public class ParticleSystem<PARTICLE> where PARTICLE : IParticle
	{
		public delegate PARTICLE ParticleCreater(float creationTime);
		public delegate void AfterParticleUpdate(PARTICLE particle);

		/// <summary>
		/// Will be called each time a particle is created
		/// </summary>
		public event ParticleCreater OnParticleCreate;
		/// <summary>
		/// Is called after a particle is updated
		/// </summary>
		public event AfterParticleUpdate OnAfterParticleUpdate;

		public ParticleSystem(int maxParticleCount)
		{
			MaxParticleCount = maxParticleCount;
			ReleaseInterval = 1f;
		}

		public void Reset()
		{
			particles.Clear();
		}

		public void Update(float time)
		{
			var deltaTime = time - lastUpdate;
			lastUpdate = time;
			if (ReferenceEquals(null, OnParticleCreate)) throw new InvalidOperationException("No OnParticleCreate handler specified!");
			var delete = new List<LinkedListNode<PARTICLE>>();
			for (var i = particles.First; i != particles.Last; i = i.Next)
			{
				var particle = i.Value;
				//update
				particle.Update(deltaTime);
				//after update callback
				OnAfterParticleUpdate?.Invoke(particle);
				//save old particles for deletion
				if (particle.CreationTime + particle.LifeTime < time)
				{
					delete.Add(i);
				}
			}
			//remove old particles
			foreach (var node in delete)
			{
				particles.Remove(node);
			}
			//if less than max particles alive -> emit new particles
			if (ParticleCount < MaxParticleCount)
			{
				while (time - lastEmit > ReleaseInterval)
				{
					lastEmit += ReleaseInterval;
					var particle = OnParticleCreate(time);
					particles.AddLast(particle);
				}
			}
		}

		public int ParticleCount { get { return particles.Count; } }
		public IEnumerable<PARTICLE> Particles { get { return particles; } }
		/// <summary>
		/// Controls how often per second a particle is created
		/// </summary>
		public float ReleaseInterval { get; set; }
		public int MaxParticleCount { get; private set; }

		private LinkedList<PARTICLE> particles = new LinkedList<PARTICLE>();
		private float lastEmit = 0f;
		private float lastUpdate = 0f;
	}
}
