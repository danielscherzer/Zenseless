using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL4;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			this.renderState = renderState;
			shaderProgram = contentLoader.Load<IShaderProgram>("lambert.*");
			shaderPostProcess = contentLoader.LoadPixelShader("ChromaticAberration");
			shaderPostProcess = contentLoader.LoadPixelShader("swirl");
			shaderPostProcess = contentLoader.LoadPixelShader("Ripple");
			var mesh = Meshes.CreateCornellBox(); //TODO: ATI seams to do VAO vertex attribute ordering different for each shader would need to create own VAO
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		private readonly IRenderState renderState;

		public void Draw(TransformationHierarchyNode camera)
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			renderState.Set(new DepthTest(true));
			renderState.Set(new BackFaceCulling(true));
			shaderProgram.Activate();
			shaderProgram.Uniform("camera", camera.CalcGlobalTransformation(), true);

			geometry.Draw();
			shaderProgram.Deactivate();
			renderState.Set(new BackFaceCulling(false));
			renderState.Set(new DepthTest(false));
		}

		public void DrawWithPostProcessing(float time, TransformationHierarchyNode camera)
		{
			renderToTexture.Activate(); //start drawing into texture
			Draw(camera);
			renderToTexture.Deactivate(); //stop drawing into texture
			renderToTexture.Texture.Activate(); //us this new texture
			if (shaderPostProcess is null) return;
			shaderPostProcess.Activate(); //activate post processing shader
			shaderPostProcess.Uniform("iGlobalTime", time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4); //draw quad
			shaderPostProcess.Deactivate();
			renderToTexture.Texture.Deactivate();
		}

		public void Resize(int width, int height)
		{
			renderToTexture = new FBOwithDepth(Texture2dGL.Create(width, height));
			renderToTexture.Texture.WrapFunction = TextureWrapFunction.ClampToEdge;
		}

		private IRenderSurface renderToTexture;
		private IShaderProgram shaderPostProcess;
		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
