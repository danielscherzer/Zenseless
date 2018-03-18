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
			drawParticles.InstanceCount = particelCount;
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

			var uniforms = new Uniforms((float)(random.NextDouble() * 2 - 1), (float)particelCount);
			drawParticles.UpdateUniforms("Uniforms", uniforms);
			frameBuffer.Draw(drawParticles);
		}

		struct Particle
		{
			public Vector2 position;
			public Vector2 velocity;
		}

		private IRenderSurface frameBuffer;
		private IRenderSurface imageObstacles;
		private IRenderSurface imageObstaclesLastFrame;
		private DrawConfiguration paintObstacles = new DrawConfiguration();
		private DrawConfiguration drawParticles = new DrawConfiguration();
		private const int particelCount = (int)1e5;
		private Random random = new Random();

		private Particle[] InitParticles()
		{
			var rnd = new Random(12);
			float RndCoord() => (float)(rnd.NextDouble() * 2 - 1);

			var data = new Particle[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				data[i].position = new Vector2(RndCoord(), RndCoord());
				data[i].velocity = new Vector2(RndCoord(), RndCoord()) * 0.01f;
			}
			return data;
		}
	}

	struct Uniforms
	{
		private float v;
		private float particelCount;

		public Uniforms(float v, float particelCount)
		{
			this.v = v;
			this.particelCount = particelCount;
		}
	}
}