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
			renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
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
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, true);
		}

		public float CameraDistance { get; set; }
		public float CameraAzimuth { get; set; }
		public float CameraElevation { get; set; }

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
#if SOLUTION
			GL.Viewport(0, 0, splitX - 2, height);
			DrawScene(view * Matrix4x4.CreateOrthographic(aspect * 10f, 10f, 0, 100f));
			GL.Viewport(splitX + 2, 0, splitX - 2, height);
#endif
			DrawScene(view * Matrix4x4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 100.0f));
		}

		public void Resize(int width, int height)
		{
			aspect = width / (float)height;
#if SOLUTION
			splitX = width / 2;
			this.height = height;
			aspect = splitX / (float)height;
#endif
		}

		private void DrawScene(Matrix4x4 camera)
		{
			shaderProgram.Activate();
			shaderProgram.Uniform(nameof(camera), camera, true);
			geometry.Draw();
			shaderProgram.Deactivate();
		}

		public void Update(float updatePeriod)
		{
			//TODO student: use CameraDistance, CameraAzimuth, CameraElevation to create an orbiting camera
			view = Matrix4x4.Identity;
#if SOLUTION
			var mtxDistance = Matrix4x4.CreateTranslation(0, 0, -CameraDistance);
			var mtxElevation = Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(CameraElevation));
			var mtxAzimut = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(CameraAzimuth));
			view = mtxAzimut * mtxElevation * mtxDistance;
#endif
		}

		private const int particleCount = 500;

		private IShaderProgram shaderProgram;
		private Matrix4x4 view = Matrix4x4.Identity;
		private VAO geometry;
		private float aspect = 1f;
#if SOLUTION
		private int splitX;
		private int height;
#endif
	}
}
