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
		public Vector2 position;
		public Vector2 velocity; //position + velocity are aligned to 16byte
		public Vector3 color;
		public float size; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
	}

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			InitParticles();
			//for transparency in textures we use blending
			renderState.Set(BlendStates.Additive);
			renderState.Set(new ShaderPointSize(true));
			renderState.Set(new PointSprite(true));

			shaderProgram = contentLoader.Load<IShaderProgram>("particle.*");
		}

		public void Render(float deltaTime)
		{
			if (shaderProgram is null) return;
			GL.PointSize(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform(nameof(deltaTime), deltaTime);
			shaderProgram.Uniform("pointResolutionScale", smallerWindowSideResolution * 0.0007f);
			var bindingIndex = shaderProgram.GetResourceLocation(ShaderResourceType.RWBuffer, "BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particleCount);
			bufferParticles.Deactivate();
			shaderProgram.Deactivate();
		}

		internal void Resize(int width, int height)
		{
			smallerWindowSideResolution = Math.Min(width, height);
		}

		private IShaderProgram shaderProgram;
		private BufferObject bufferParticles;
		private int smallerWindowSideResolution;
		private const int particleCount = (int)1e4;

		private void InitParticles()
		{
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			float RndSpeed() => (Rnd01() - 0.5f) * 0.1f;

			bufferParticles = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Particle[particleCount];
			for (int i = 0; i < particleCount; ++i)
			{
				data[i].position = new Vector2(RndCoord(), RndCoord());
				data[i].velocity = new Vector2(RndSpeed(), RndSpeed());
				var polar = MathHelper.ToPolar(data[i].position);
				var color = ColorSystems.Hsb2rgb(polar.X / MathHelper.TWO_PI + 0.5f, polar.Y, 1);
				var byteColor = ColorSystems.ToSystemColor(color);
				data[i].color = color;
				data[i].size = (Rnd01() + 1) * 10;
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
