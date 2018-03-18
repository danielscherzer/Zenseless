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
			renderState.Set(BoolState<IShaderPointSizeState>.Enabled);
			renderState.Set(BoolState<IPointSpriteState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("particle.*");
		}

		public void Render(float deltaTime)
		{
			if (shaderProgram is null) return;
			GL.PointSize(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shaderProgram.Activate();
			GL.Uniform1(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "deltaTime"), deltaTime);
			GL.Uniform1(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "particelCount"), particelCount);
			var bindingIndex = shaderProgram.GetResourceLocation(ShaderResourceType.RWBuffer, "BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			bufferParticles.Deactivate();
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private BufferObject bufferParticles;
		private const int particelCount = (int)1e4;

		private void InitParticles()
		{
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			float RndSpeed() => (Rnd01() - 0.5f) * 0.1f;

			bufferParticles = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Particle[particelCount];
			for (int i = 0; i < particelCount; ++i)
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
