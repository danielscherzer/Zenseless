using System.Collections.Generic;
using System.Numerics;
using Zenseless.ExampleFramework;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit Camera { get; private set; } = new CameraOrbit();

		public MainVisual(IRenderContext context, IContentLoader contentLoader)
		{
			Camera.FarClip = 50f;
			Camera.Distance = 1.5f;
			Camera.FovY = 90f;
			Camera.Azimuth = 90;
			Camera.Elevation = 20;

			this.context = context;
			frameBuffer = context.GetFrameBuffer();
			surfaceGeometry = context.CreateRenderSurface(512, 512, true);
			suzanne.BackfaceCulling = true;
			suzanne.SetInputTexture("chalet", contentLoader.Load<ITexture2D>("chalet.jpg"));
			//model from https://sketchfab.com/models/e925320e1d5744d9ae661aeff61e7aef
			var mesh = contentLoader.Load<DefaultMesh>("chalet.obj").Transform(Matrix4x4.CreateRotationX(-0.5f * MathHelper.PI));
			suzanne.UpdateMeshShader(mesh, contentLoader.Load<IShaderProgram>("shader.*"));
			suzanne.ZBufferTest = true;

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
			suzanne.UpdateInstanceAttribute("translate", translates.ToArray());
			suzanne.InstanceCount = translates.Count;
		}

		internal void Resize(int width, int height)
		{
			surfaceGeometry = context.CreateRenderSurface(width, height, true);
			copyQuad.SetInputTexture("tex", surfaceGeometry);
		}

		public void Render()
		{
			surfaceGeometry.Clear();

			uniforms.camera = Matrix4x4.Transpose(Camera.CalcMatrix());
			suzanne.UpdateUniforms(nameof(Uniforms), uniforms);
			surfaceGeometry.Draw(suzanne);

			frameBuffer.Draw(copyQuad);
		}

		private struct Uniforms
		{
			public Matrix4x4 camera;
		};

		private IRenderContext context;
		private IRenderSurface frameBuffer;
		private IRenderSurface surfaceGeometry;
		private Uniforms uniforms = new Uniforms();
		private DrawConfiguration suzanne = new DrawConfiguration();
		private DrawConfiguration copyQuad = new DrawConfiguration();
	}
}
