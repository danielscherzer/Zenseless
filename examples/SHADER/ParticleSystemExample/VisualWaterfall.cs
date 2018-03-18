using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Example
{
	public class VisualWaterfall
	{
		public VisualWaterfall(Vector3 emitterPos, IRenderState renderState, IContentLoader contentLoader)
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
			var p = new Particle(creationTime);
			p.LifeTime = 5f;
			float Rnd01() => (float)random.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
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
				var direction = new Vector3(RndCoord(), Rnd01(), RndCoord()).Normalized();
				var speed = particle.Velocity.Length;
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

			particles.SetAttribute(shaderWaterfall.GetResourceLocation(ShaderResourceType.Attribute, "position"), positions, VertexAttribPointerType.Float, 3);
			particles.SetAttribute(shaderWaterfall.GetResourceLocation(ShaderResourceType.Attribute, "fade"), fade, VertexAttribPointerType.Float, 1);
		}

		public void Render(Matrix4 camera)
		{
			if (shaderWaterfall is null) return;
			renderState.Set(BlendStates.Additive);
			GL.DepthMask(false);
			renderState.Set(BoolState<IPointSpriteState>.Enabled);
			renderState.Set(BoolState<IShaderPointSizeState>.Enabled);

			shaderWaterfall.Activate();
			GL.UniformMatrix4(shaderWaterfall.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref camera);
			GL.Uniform1(shaderWaterfall.GetResourceLocation(ShaderResourceType.Uniform, "pointSize"), 0.3f);
			//GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "texParticle"), 0);
			texStar.Activate();
			particles.Draw();
			texStar.Deactivate();
			shaderWaterfall.Deactivate();

			renderState.Set(BoolState<IShaderPointSizeState>.Disabled);
			renderState.Set(BoolState<IPointSpriteState>.Disabled);
			renderState.Set(BlendStates.Opaque);
			GL.DepthMask(true);
		}

		private IShaderProgram shaderWaterfall;
		private ITexture texStar;
		private VAO particles = new VAO(PrimitiveType.Points);
		private ParticleSystem<Particle> particleSystem = new ParticleSystem<Particle>(10000);
		private Random random = new Random();
		private readonly IRenderState renderState;
		private readonly Vector3 emitterPos;
	}
}
