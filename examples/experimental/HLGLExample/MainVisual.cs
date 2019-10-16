﻿namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.ExampleFramework;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class MainVisual
	{
		public MainVisual(IRenderContext context, IContentLoader contentLoader)
		{
			this.context = context;
			frameBuffer = context.GetFrameBuffer();
			surfaceGeometry = context.CreateRenderSurface(1024, 1024, true);
			this.scene.BackfaceCulling = true;
			this.scene.SetInputTexture("chalet", contentLoader.Load<ITexture2D>("chalet.jpg"));
			//model from https://sketchfab.com/models/e925320e1d5744d9ae661aeff61e7aef
			var mesh = contentLoader.Load<DefaultMesh>("chalet.obj").Transform(Transformation.Rotation(-90f, Axis.X));
			this.scene.UpdateMeshShader(mesh, contentLoader.Load<IShaderProgram>("shader.*"));
			this.scene.ZBufferTest = true;

			copyQuad.BackfaceCulling = false;
			copyQuad.SetInputTexture("tex", surfaceGeometry);
			copyQuad.UpdateMeshShader(null, contentLoader.LoadPixelShader("copy.frag"));
			copyQuad.ZBufferTest = false;

			var delta = 2f;
			var extend = 1f * delta;
			var translates = new List<Vector3>();
			for (float x = -extend; x <= extend; x += delta)
			{
				for (float z = -extend; z <= extend; z += delta)
				{
					translates.Add(new Vector3(x, 0, z));
				}
			}
			this.scene.UpdateInstanceAttribute("translate", translates.ToArray());
			this.scene.InstanceCount = translates.Count;
		}

		internal void Resize(int width, int height)
		{
			surfaceGeometry = context.CreateRenderSurface(width, height, true);
			copyQuad.SetInputTexture("tex", surfaceGeometry);
		}

		public void Render(ITransformation camera)
		{
			surfaceGeometry.Clear();

			uniformBlock.camera = camera.Matrix;
			scene.UpdateUniformBuffer("Uniforms", uniformBlock);
			surfaceGeometry.Draw(scene);

			frameBuffer.Draw(copyQuad);
		}

		private struct UniformBlock
		{
			public Matrix4x4 camera; //memory layout is row-major so transposed to GL
		};

		private IRenderContext context;
		private IOldRenderSurface frameBuffer;
		private IOldRenderSurface surfaceGeometry;
		private UniformBlock uniformBlock = new UniformBlock();
		private DrawConfiguration scene = new DrawConfiguration();
		private DrawConfiguration copyQuad = new DrawConfiguration();
	}
}
