using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	//[StructLayout(LayoutKind.Sequential, Pack = 1)] // does not help with required shader alignment, affects only CPU part
	struct Particle //use 16 byte alignment or you have to query all variable offsets
	{
		public Vector3 position;
		public float size; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
		public Vector3 velocity;
		public float age; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
	}

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			InitParticles();
			renderState.Set(BlendStates.Additive);
			renderState.Set(new ShaderPointSize(true));
			renderState.Set(new PointSprite(true));
			shaderProgram = contentLoader.Load<IShaderProgram>("particle.*");
			texSmoke = contentLoader.Load<ITexture2D>("smoke_256a");
		}

		public void Render(float deltaTime, ITransformation camera)
		{
			if (shaderProgram is null) return;
			time += deltaTime;
			if (time > 2f)
			{
				acceleration = 4f * new Vector3(Rnd(-1, 1), Rnd(0, 1), Rnd(-1, 1));
				time = 0f;
			}

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			texSmoke.Activate();
			shaderProgram.Activate();
			shaderProgram.Uniform("camera", camera);
			shaderProgram.Uniform(nameof(deltaTime), deltaTime);
			shaderProgram.Uniform(nameof(lifeTime), lifeTime);
			shaderProgram.Uniform(nameof(source), source);
			shaderProgram.Uniform(nameof(acceleration), acceleration);
			shaderProgram.Uniform(nameof(resetVelocityLowerBounds), resetVelocityLowerBounds);
			shaderProgram.Uniform(nameof(resetVelocityUpperBounds), resetVelocityUpperBounds);
			shaderProgram.Uniform("pointResolutionScale", smallerWindowSideResolution * 0.07f);
			var bindingIndex = shaderProgram.GetResourceLocation(ShaderResourceType.RWBuffer, "BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particleCount);
			shaderProgram.Deactivate();
			bufferParticles.Deactivate();
			texSmoke.Deactivate();
		}

		internal void Resize(int width, int height)
		{
			smallerWindowSideResolution = Math.Min(width, height);
		}

		private const int particleCount = 3000;
		private readonly IShaderProgram shaderProgram;
		private readonly ITexture2D texSmoke;
		private readonly float lifeTime = 4f;
		private readonly Vector3 resetVelocityLowerBounds = new Vector3(-0.6f, 1f, -0.6f);
		private readonly Vector3 resetVelocityUpperBounds = new Vector3(0.6f, 2f, 0.6f);
		private Vector3 source = Vector3.Zero;
		private Vector3 acceleration = new Vector3(0, 0, 0);
		private BufferObject bufferParticles;
		private readonly Random rnd = new Random(12);
		private float smallerWindowSideResolution;
		private float time = 0f;

		private float Rnd01() => (float)rnd.NextDouble();
		private float Rnd(float min, float max) => min + Rnd01() * (max - min);
		private Vector3 RndV3() => new Vector3(Rnd01(), Rnd01(), Rnd01());
		private Vector3 Rnd(Vector3 min, Vector3 max) => min + RndV3() * (max - min);

		private void InitParticles()
		{
			bufferParticles = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Particle[particleCount];
			for (int i = 0; i < particleCount; ++i)
			{
				data[i].position = source;
				data[i].velocity = Rnd(resetVelocityLowerBounds, resetVelocityUpperBounds);
				data[i].age = Rnd(0f, lifeTime);
				data[i].size = Rnd(30f, 400f);
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
