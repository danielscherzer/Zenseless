using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderContext context, IContentLoader contentLoader)
		{
			camera.FarClip = 20;
			camera.Distance = 3;
			camera.FovY = 70;
			camera.Elevation = 15;

			context.RenderState.Set(BoolState<IDepthState>.Enabled);
			context.RenderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("particle.*");
			//geometry = VAOLoader.FromMesh(Meshes.CreateSphere(0.01f, 2), shader);
			geometry = VAOLoader.FromMesh(contentLoader.Load<DefaultMesh>("suzanne").Transform(Matrix4x4.CreateScale(0.01f)), shaderProgram);

			InitParticles();
		}

		public static readonly string ShaderName = nameof(shaderProgram);

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Render()
		{
			if (shaderProgram is null) return;

			for (int i = 0; i < instanceCount; ++i)
			{
				instancePosition[i] += instanceVelocity[i];
				var abs = Vector3.One - Vector3.Abs(instancePosition[i]);
				if(abs.X < 0 || abs.Y < 0 || abs.Z < 0)
				{
					instanceVelocity[i] = -instanceVelocity[i];
				}
				instanceRotation[i] += 0.2f;
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePosition, VertexAttribPointerType.Float, 3, true);
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceRotation"), instanceRotation, VertexAttribPointerType.Float, 1, true);


			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			geometry.Draw(instanceCount);
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private const int instanceCount = (int)30000;
		private Vector3[] instancePosition = new Vector3[instanceCount];
		private Vector3[] instanceVelocity = new Vector3[instanceCount];
		private float[] instanceRotation = new float[instanceCount];
		private Vector3[] instanceColor = new Vector3[instanceCount];
		private CameraOrbit camera = new CameraOrbit();
		private VAO geometry;

		private void InitParticles()
		{
			//per instance attributes
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			float RndSpeed() => (Rnd01() - 0.5f) * 0.01f;
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePosition[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
				instanceVelocity[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
				instanceColor[i] = new Vector3(0.5f) + instancePosition[i] * 0.5f;
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePosition, VertexAttribPointerType.Float, 3, true);
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceColor"), instanceColor, VertexAttribPointerType.Float, 3, true);
		}
	}
}
