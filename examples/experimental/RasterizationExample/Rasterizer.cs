using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Example
{
	public class Rasterizer
	{
		public Rasterizer(IContentLoader contentLoader, int resolutionX, int resolutionY, Action drawHandler)
		{
			if (drawHandler is null)
			{
				throw new ArgumentException("Draw handler must not equal null!");
			}

			this.drawHandler = drawHandler;
			copyToFrameBuffer = new PostProcessing(contentLoader.LoadPixelShader("copy.frag"));
			var texRenderSurface = Texture2dGL.Create(resolutionX, resolutionY);
			texRenderSurface.Filter = TextureFilterMode.Nearest;
			fbo = new FBO(texRenderSurface);
		}

		private IRenderSurface fbo;
		private PostProcessing copyToFrameBuffer;
		private readonly Action drawHandler;

		public void Render()
		{
			fbo.Execute(drawHandler);
			copyToFrameBuffer.Draw(fbo.Texture);
		}
	}
}
