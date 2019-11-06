﻿using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)] // does not help with required shader alignment, affects only cpu part
	struct Particle //use 16 byte alignment or you have to query all variable offsets
	{
		public Vector3 position;
		public float size; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
		public Vector3 velocity;
		public uint color; //float is aligned with previous vec3 to 16byte alignment, changing the order does not work
	}

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			InitParticles();
			renderState.Set(new DepthTest(true));
			renderState.Set(new ShaderPointSize(true));
			renderState.Set(new PointSprite(true));

			shaderProgram = contentLoader.Load<IShaderProgram>("particle.*");
		}

		public double Render(float deltaTime, ITransformation camera)
		{
			if (shaderProgram is null) return 0;
			var timerQueryResult = timeQuery.ResultLong * 1e-6;
			timeQuery.Activate(QueryTarget.TimeElapsed);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform("camera", camera);
			shaderProgram.Uniform(nameof(deltaTime), deltaTime);
			shaderProgram.Uniform("pointResolutionScale", smallerWindowSideResolution * 0.001f);
			var bindingIndex = shaderProgram.GetResourceLocation(ShaderResourceType.RWBuffer, "BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particleCount);
			bufferParticles.Deactivate();
			shaderProgram.Deactivate();
			timeQuery.Deactivate();
			return timerQueryResult;
		}

		internal void Resize(int width, int height)
		{
			smallerWindowSideResolution = Math.Min(width, height);
		}

		private IShaderProgram shaderProgram;
		private BufferObject bufferParticles;
		private QueryObject timeQuery = new QueryObject();
		private int smallerWindowSideResolution;
		private const int particleCount = (int)1e5;

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
				var pos = new Vector3(RndCoord(), RndCoord(), RndCoord());
				data[i].position = pos;
				data[i].velocity = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
				var color = new Vector4(new Vector3(0.5f) + pos * 0.5f, 1);
				var packedColor = MathHelper.PackUnorm4x8(color);
				data[i].color = packedColor;
				data[i].size = (Rnd01() + 1) * 10;
			}
			bufferParticles.Set(data, BufferUsageHint.StaticCopy);
		}
	}
}
