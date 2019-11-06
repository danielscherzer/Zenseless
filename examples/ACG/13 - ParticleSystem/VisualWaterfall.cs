using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class VisualWaterfall
	{
		public VisualWaterfall(in Vector3 emitterPos, IRenderState renderState, IContentLoader contentLoader)
		{
			this.renderState = renderState;
			this.emitterPos = emitterPos;
			texStar = contentLoader.Load<ITexture2D>("water_splash");
			shaderWaterfall = contentLoader.Load<IShaderProgram>("particle.*");
			particleSystem.ReleaseInterval = 0.003f;
			particleSystem.OnParticleCreate += Create;
			particleSystem.OnAfterParticleUpdate += OnAfterParticleUpdate;
		}

		public Particle Create(float creationTime)
		{
			var p = new Particle(creationTime)
			{
				LifeTime = 5f
			};
			float Rnd01() => (float)random.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			//around emitter position
			p.Position = emitterPos + new Vector3(RndCoord(), RndCoord(), RndCoord()) * .1f;
			//start speed
			p.Velocity = Vector3.Zero;
			//downward gravity
			p.Acceleration = new Vector3(0, -.4f, 0);
			return p;
		}

		private void OnAfterParticleUpdate(Particle particle)
		{
			float Rnd01() => (float)random.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;

			//if collision with ground plane
			if (particle.Position.Y < 0)
			{
				//slightly different upward vectors
				var direction = Vector3.Normalize(new Vector3(RndCoord(), Rnd01(), RndCoord()));
				var speed = particle.Velocity.Length();
				//random perturb velocity to get more water like effects
				particle.Velocity = direction * speed * 0.7f;
			}
		}

		public void Update(float time)
		{
			if (shaderWaterfall is null) return;
			particleSystem.Update(time);
			//gather all active particle positions into array
			var positions = new Vector3[particleSystem.ParticleCount];
			var fade = new float[particleSystem.ParticleCount];
			int i = 0;
			foreach (var particle in particleSystem.Particles)
			{
				//fading with age effect
				var age = time - particle.CreationTime;
				fade[i] = 1f - age / particle.LifeTime;
				positions[i] = particle.Position;
				++i;
			}

			particles.SetAttribute(shaderWaterfall.GetResourceLocation(ShaderResourceType.Attribute, "position"), positions);
			particles.SetAttribute(shaderWaterfall.GetResourceLocation(ShaderResourceType.Attribute, "fade"), fade);
		}

		public void Render(in ITransformation camera)
		{
			if (shaderWaterfall is null) return;
			renderState.Set(BlendStates.Additive);
			GL.DepthMask(false);
			renderState.Set(new PointSprite(true));
			renderState.Set(new ShaderPointSize(true));

			shaderWaterfall.Activate();
			shaderWaterfall.Uniform(nameof(camera), camera);
			shaderWaterfall.Uniform("pointSize", smallerWindowSideResolution * 0.00083f);
			//shader.Uniform("texParticle", 0);
			texStar.Activate();
			particles.Draw();
			texStar.Deactivate();
			shaderWaterfall.Deactivate();

			renderState.Set(new ShaderPointSize(false));
			renderState.Set(new PointSprite(false));
			renderState.Set(BlendStates.Opaque);
			GL.DepthMask(true);
		}

		internal void Resize(int width, int height)
		{
			smallerWindowSideResolution = Math.Min(width, height);
		}

		private IShaderProgram shaderWaterfall;
		private ITexture texStar;
		private VAO particles = new VAO(PrimitiveType.Points);
		private ParticleSystem<Particle> particleSystem = new ParticleSystem<Particle>(10000);
		private Random random = new Random();
		private int smallerWindowSideResolution;
		private readonly IRenderState renderState;
		private readonly Vector3 emitterPos;
	}
}
