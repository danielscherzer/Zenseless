using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.Patterns;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class PostProcessingVisual
	{
		public PostProcessingVisual(int width, int height, IContentLoader contentLoader)
		{
			renderToTexture = new FBO(Texture2dGL.Create(width, height));
			renderToTexture.Texture.WrapFunction = TextureWrapFunction.MirroredRepeat;
			try
			{
				shaderProgram = contentLoader.LoadPixelShader("Grayscale");
				shaderProgram = contentLoader.LoadPixelShader("Sepia");
				shaderProgram = contentLoader.LoadPixelShader("Vignetting");
				shaderProgram = contentLoader.LoadPixelShader("ChromaticAberration");
				shaderProgram = contentLoader.LoadPixelShader("convolution");
				shaderProgram = contentLoader.LoadPixelShader("EdgeDetect");
				shaderProgram = contentLoader.LoadPixelShader("Ripple");
				shaderProgram = contentLoader.LoadPixelShader("Swirl");
			}
			catch (ShaderException e)
			{
				Console.WriteLine(e.ShaderLog);
				Console.ReadLine();
			}
		}

		public void Render(Action renderAction)
		{
			//render into texture
			renderToTexture.Activate();
			renderAction.Invoke();
			renderToTexture.Deactivate();

			//use this texture to draw
			renderToTexture.Texture.Activate();
			shaderProgram.Activate();
			//SetShaderParameter("effectScale", 0.1f);
			shaderProgram.Uniform("effectScale", 0.5f + 0.5f * (float)Math.Sin(time.AbsoluteTime - 0.5f));
			shaderProgram.Uniform("iGlobalTime", time.AbsoluteTime);
			DrawWindowFillingQuad();
			shaderProgram.Deactivate();
			renderToTexture.Texture.Deactivate();
		}

		private GameTime time = new GameTime();
		private IRenderSurface renderToTexture;
		private IShaderProgram shaderProgram;

		private static void DrawWindowFillingQuad()
		{
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
		}
	}
}
