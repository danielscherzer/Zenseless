namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			this.renderState = renderState;
			shaderProgram = contentLoader.Load<IShaderProgram>("lambert.*");
			shaderPostProcess = contentLoader.LoadPixelShader("ChromaticAberration");
			shaderPostProcess = contentLoader.LoadPixelShader("swirl");
			shaderPostProcess = contentLoader.LoadPixelShader("sepia");
			shaderPostProcess = contentLoader.LoadPixelShader("vignet");
			shaderPostProcess = contentLoader.LoadPixelShader("Ripple");

			var mesh = Meshes.CreateCornellBox(); //TODO: ATI seams to do VAO vertex attribute ordering different for each shader would need to create own VAO
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		private readonly IRenderState renderState;

		public void Draw(ITransformation camera)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			renderState.Set(new DepthTest(true));
			renderState.Set(new BackFaceCulling(true));
			shaderProgram.Activate();
			shaderProgram.Uniform("camera", camera);
			geometry.Draw();
			shaderProgram.Deactivate();
			renderState.Set(new BackFaceCulling(false));
			renderState.Set(new DepthTest(false));
		}

		public void DrawWithPostProcessing(float time, ITransformation camera)
		{
			renderToTexture.Activate(); //start drawing into texture
			Draw(camera);
			renderToTexture.Deactivate(); //stop drawing into texture
			renderToTexture.Texture.Activate(); //use this new texture
			shaderPostProcess.Activate(); //activate post processing shader
			shaderPostProcess.Uniform("iGlobalTime", time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4); //draw quad
			shaderPostProcess.Deactivate();
			renderToTexture.Texture.Deactivate();
		}

		public void Resize(int width, int height)
		{
			renderToTexture = new FBOwithDepth(Texture2dGL.Create(width, height));
			renderToTexture.Texture.WrapFunction = TextureWrapFunction.MirroredRepeat;
		}

		private IRenderSurface renderToTexture;
		private IShaderProgram shaderPostProcess;
		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
