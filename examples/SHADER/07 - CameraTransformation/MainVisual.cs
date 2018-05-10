using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			CameraDistance = 10.0f;
			renderState.Set(new DepthTest(true));
			renderState.Set(new BackFaceCulling(true));
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);

			//per instance attributes
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 35.0f;
			var instancePositions = new Vector3[particleCount];
			for (int i = 0; i < particleCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);
		}

		public float CameraDistance { get; set; }
		public float CameraAzimuth { get; set; }
		public float CameraElevation { get; set; }

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform(nameof(camera), camera, true);
			geometry.Draw(particleCount);
			shaderProgram.Deactivate();
		}

		public void Update(float updatePeriod)
		{
			//TODO student: use CameraDistance, CameraAzimuth, CameraElevation
			var p = Matrix4x4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100.0f);
			camera = p;
		}

		private const int particleCount = 500;

		private IShaderProgram shaderProgram;
		private Matrix4x4 camera = Matrix4x4.Identity;
		private VAO geometry;
	}
}
