using System;
using System.Numerics;
using Zenseless.ExampleFramework;
using Zenseless.HLGL;

namespace Example
{
	public struct MouseState
	{
		public Vector2 position;
		public int drawState;
	};

	public class MainVisual
	{
		public MouseState MouseState { get; set; }

		public MainVisual(IRenderContext renderContext, IContentLoader contentLoader)
		{
			frameBuffer = renderContext.GetFrameBuffer();
			imageObstacles = renderContext.CreateRenderSurface(512, 512, false, 2, true);
			imageObstaclesLastFrame = renderContext.CreateRenderSurface(512, 512, false, 2, true);

			paintObstacles.UpdateMeshShader(null, contentLoader.LoadPixelShader("paintObstacles"));

			drawParticles.ShaderPointSize = true;
			drawParticles.UpdateShaderBuffer("Particles", InitParticles());
			drawParticles.UpdateMeshShader(null, contentLoader.Load<IShaderProgram>("particles.*"));
			drawParticles.InstanceCount = particleCount;
		}

		public void Render()
		{
			paintObstacles.UpdateUniforms(nameof(MouseState), this.MouseState);
			paintObstacles.SetInputTexture("texObstacles", imageObstaclesLastFrame);
			imageObstacles.Draw(paintObstacles);

			//swap
			var temp = imageObstacles;
			imageObstacles = imageObstaclesLastFrame;
			imageObstaclesLastFrame = temp;

			frameBuffer.Clear();
			drawParticles.SetInputTexture("texObstacles", imageObstacles);
			frameBuffer.Draw(paintObstacles);

			var uniforms = new Uniforms((float)(random.NextDouble() * 2 - 1), (float)particleCount);
			drawParticles.UpdateUniforms("Uniforms", uniforms);
			frameBuffer.Draw(drawParticles);
		}

		struct Particle
		{
			public Vector2 position;
			public Vector2 velocity;
		}

		private IOldRenderSurface frameBuffer;
		private IOldRenderSurface imageObstacles;
		private IOldRenderSurface imageObstaclesLastFrame;
		private DrawConfiguration paintObstacles = new DrawConfiguration();
		private DrawConfiguration drawParticles = new DrawConfiguration();
		private const int particleCount = (int)1e5;
		private Random random = new Random();

		private Particle[] InitParticles()
		{
			var rnd = new Random(12);
			float RndCoord() => (float)(rnd.NextDouble() * 2 - 1);

			var data = new Particle[particleCount];
			for (int i = 0; i < particleCount; ++i)
			{
				data[i].position = new Vector2(RndCoord(), RndCoord());
				data[i].velocity = new Vector2(RndCoord(), RndCoord()) * 0.01f;
			}
			return data;
		}
	}

	struct Uniforms
	{
		private readonly float v;
		private readonly float particleCount;

		public Uniforms(float v, float particleCount)
		{
			this.v = v;
			this.particleCount = particleCount;
		}
	}
}