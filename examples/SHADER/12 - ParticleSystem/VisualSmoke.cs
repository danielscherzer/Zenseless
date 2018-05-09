using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class VisualSmoke
	{
		private readonly IRenderState renderState;

		public Vector3 Emitter { get; set; }
		public Vector3 Wind { get; set; }

		public VisualSmoke(Vector3 emitterPos, Vector3 wind, IRenderState renderState, IContentLoader contentLoader)
		{
			this.renderState = renderState;

			Emitter = emitterPos;
			Wind = wind;
			texSmoke = contentLoader.Load<ITexture2D>("smoke_256a");
			shaderSmoke = contentLoader.Load<IShaderProgram>("particle.*");
			particleSystem.ReleaseInterval = 0.01f;
			particleSystem.OnParticleCreate += Create;
		}

		public Particle Create(float creationTime)
		{
			var p = new Particle(creationTime)
			{
				LifeTime = 10f
			};
			float Rnd01() => (float)random.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			//start at emitter position
			p.Position = Emitter;
			//slightly different upward vectors
			var direction = new Vector3(0.3f * RndCoord(), 1, 0.3f * RndCoord());
			//speed between [.2, .4]
			p.Velocity = (.2f + .2f * Rnd01()) * direction;
			p.Acceleration = Wind;
			return p;
		}

		internal void Resize(int width, int height)
		{
			smallerWindowSideResolution = Math.Min(width, height);
		}

		public void Update(float time)
		{
			if (shaderSmoke is null) return;
			particleSystem.Update(time);
			//gather all active particle positions into array
			var positions = new Vector3[particleSystem.ParticleCount];
			var fade = new float[particleSystem.ParticleCount];
			int i = 0;
			foreach (var particle in particleSystem.Particles)
			{
				var p = particle as Particle;
				var age = time - p.CreationTime;
				fade[i] = 1f - age / p.LifeTime;
				positions[i] = p.Position;
				++i;
			}

			particles.SetAttribute(shaderSmoke.GetResourceLocation(ShaderResourceType.Attribute, "position"), positions, VertexAttribPointerType.Float, 3);
			particles.SetAttribute(shaderSmoke.GetResourceLocation(ShaderResourceType.Attribute, "fade"), fade, VertexAttribPointerType.Float, 1);
		}

		public void Render(in Matrix4x4 camera)
		{
			if (shaderSmoke is null) return;
			renderState.Set(BlendStates.Additive);
			GL.DepthMask(false);
			renderState.Set(BoolState<IPointSpriteState>.Enabled);
			renderState.Set(BoolState<IShaderPointSizeState>.Enabled);

			shaderSmoke.Activate();
			shaderSmoke.Uniform(nameof(camera), camera);
			shaderSmoke.Uniform("pointSize", smallerWindowSideResolution * 0.002f);
			//shader.Uniform("texParticle", 0);
			texSmoke.Activate();
			particles.Draw();
			texSmoke.Deactivate();
			shaderSmoke.Deactivate();

			renderState.Set(BoolState<IShaderPointSizeState>.Disabled);
			renderState.Set(BoolState<IPointSpriteState>.Disabled);
			renderState.Set(BlendStates.Opaque);
			GL.DepthMask(true);
		}

		private IShaderProgram shaderSmoke;
		private ITexture texSmoke;
		private VAO particles = new VAO(PrimitiveType.Points);
		private ParticleSystem<Particle> particleSystem = new ParticleSystem<Particle>(1000);
		private Random random = new Random();
		private int smallerWindowSideResolution;
	}
}
