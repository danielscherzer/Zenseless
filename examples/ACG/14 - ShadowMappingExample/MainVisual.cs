namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new DepthTest(true));
			fboShadowMap.Texture.Filter = TextureFilterMode.Nearest;

			shaderProgram = contentLoader.Load<IShaderProgram>("shadowMap.*");
			var mesh = Meshes.CreatePlane(10, 10, 10, 10);
			var sphere = Meshes.CreateSphere(0.5f, 2);
			sphere.SetConstantUV(new Vector2(0.5f, 0.5f));
			mesh.Add(sphere.Transform(Transformation.Translation(0, 2, -2)));
			mesh.Add(sphere.Transform(Transformation.Translation(0, 2, 0)));
			mesh.Add(sphere.Transform(Transformation.Translation(2, 2, -1)));
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);

			shaderProgramDepth = contentLoader.Load<IShaderProgram>("depth.*");
			//todo: radeon cards created errors with geometry bound to one shader and used in other shaders because of binding id changes
		}

		public Camera<Orbit, Perspective> Camera { get; } = new Camera<Orbit, Perspective>(new Orbit(8, 0, 30), new Perspective(90, 0.1f, 50f));

		public void Render()
		{
			if (shaderProgram is null) return;
			if (shaderProgramDepth is null) return;
			//first pass: create shadow map
#if SOLUTION
			shaderProgramDepth.Activate();
			fboShadowMap.Execute(() =>
			{
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
				shaderProgramDepth.Uniform("camera", light);
				geometry.Draw();
				shaderProgramDepth.Deactivate();
			});
			//TextureDebugger.Draw(fboShadowMap.Texture); return;
#endif
			//second pass: render with shadow map
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			fboShadowMap.Texture.Activate();
			shaderProgram.Uniform("ambient", new Vector3(0.1f, 0.1f, 0f));
			shaderProgram.Uniform("camera", Camera);
#if SOLUTION
			shaderProgram.Uniform(nameof(light), light);
			shaderProgram.Uniform("lightPosition", light.View.CalcPosition());
#endif
			geometry.Draw();
			fboShadowMap.Texture.Deactivate();
			shaderProgram.Deactivate();
		}

		private readonly Camera<Orbit, Perspective> light = new Camera<Orbit, Perspective>(new Orbit(8, -100, 44), new Perspective(farClip:50));
		private IShaderProgram shaderProgram;
		private IShaderProgram shaderProgramDepth;
		private IRenderSurface fboShadowMap = new FBOwithDepth(Texture2dGL.Create(2048, 2048, 1, true));
		private IDrawable geometry;
	}
}
