using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	//[StructLayout(LayoutKind.Sequential, Pack = 1)] // does not help with required shader alignment, affects only cpu part
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
			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(new ShaderPointSize(true));
			renderState.Set(new PointSprite(true));
			shaderProgram = contentLoader.Load<IShaderProgram>("particle.*");
		}

		public void Render(float deltaTime, ITransformation camera)
		{
			if (shaderProgram is null) return;
			//if ((destination - source).LengthSquared() < 0.01)
			//{
			//	destination = new Vector3(RndCoord(), RndCoord(), RndCoord());
			//}
			//else
			//{
			//	source = MathHelper.Lerp(source, destination, 0.005f);
			//}
			//camera.Azimuth += 0.5f;
			//camera.Elevation += 0.1f;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Uniform("camera", camera);
			shaderProgram.Uniform(nameof(deltaTime), deltaTime);
			shaderProgram.Uniform(nameof(source), source);
			shaderProgram.Uniform(nameof(acceleration), acceleration);
			shaderProgram.Uniform(nameof(particleCount), particleCount);
			shaderProgram.Uniform("pointResolutionScale", smallerWindowSideResolution * 0.07f);
			var bindingIndex = shaderProgram.GetResourceLocation(ShaderResourceType.RWBuffer, "BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			shaderProgram.Activate();
			GL.DrawArrays(PrimitiveType.Points, 0, particleCount);
			shaderProgram.Deactivate();
			bufferParticles.Deactivate();
		}

		internal void Resize(int width, int height)
		{
			smallerWindowSideResolution = Math.Min(width, height);
		}

		private Vector3 source = Vector3.Zero;
		private Vector3 destination = Vector3.One;
		private Vector3 acceleration = new Vector3(0, 0, 0);
		private IShaderProgram shaderProgram;
		private BufferObject bufferParticles;
		private const int particleCount = (int)1e5;
		private Random rnd = new Random(12);
		private float smallerWindowSideResolution;

		private float Rnd01() => (float)rnd.NextDouble();
		private float RndCoord() => Rnd01() * 2 - 1;

		private void InitParticles()
		{
			bufferParticles = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Particle[particleCount];
			for (int i = 0; i < particleCount; ++i)
			{
				data[i].position = source;

				var direction = new Vector3(0.3f * RndCoord(), 1, 0.3f * RndCoord());

				data[i].velocity = (.02f + .06f * Rnd01()) * direction;

				data[i].age = Rnd01() * 10;
				data[i].size = (Rnd01() + 1) * 10;
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
