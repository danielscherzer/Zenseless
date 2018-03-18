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
			camera.FarClip = 20;
			camera.Distance = 2;
			camera.FovY = 70;
			camera.Elevation = 15;

			InitParticles();
			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(BoolState<IShaderPointSizeState>.Enabled);
			renderState.Set(BoolState<IPointSpriteState>.Enabled);
			shaderProgram = contentLoader.Load<IShaderProgram>("particle.*");
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Render(float deltaTime)
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
			shaderProgram.Activate();
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform1(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "deltaTime"), deltaTime);
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "source"), source.ToOpenTK());
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "acceleration"), acceleration.ToOpenTK());
			GL.Uniform1(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "particelCount"), particelCount);
			var bindingIndex = shaderProgram.GetResourceLocation(ShaderResourceType.RWBuffer, "BufferParticle");
			bufferParticles.ActivateBind(bindingIndex);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			bufferParticles.Deactivate();
			shaderProgram.Deactivate();
		}

		private Vector3 source = Vector3.Zero;
		private Vector3 destination = Vector3.One;
		private Vector3 acceleration = new Vector3(0, 0, 0);
		private IShaderProgram shaderProgram;
		private BufferObject bufferParticles;
		private const int particelCount = (int)1e5;
		private CameraOrbit camera = new CameraOrbit();
		private Random rnd = new Random(12);

		private float Rnd01() => (float)rnd.NextDouble();
		private float RndCoord() => Rnd01() * 2 - 1;

		private void InitParticles()
		{
			bufferParticles = new BufferObject(BufferTarget.ShaderStorageBuffer);

			var data = new Particle[particelCount];
			for (int i = 0; i < particelCount; ++i)
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
