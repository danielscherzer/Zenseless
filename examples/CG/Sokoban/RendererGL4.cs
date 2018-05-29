using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using Zenseless.HLGL;

namespace Example
{
	public class RendererGL4 : IRenderer
	{
		public RendererGL4(IRenderState renderState, IContentLoader contentLoader)
		{
			//for transparency in textures we use blending
			renderState.Set(BlendStates.AlphaBlend);

			levelVisual = new VisualLevel(contentLoader);
			font = new FontGL(contentLoader);
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void DrawLevelState(ILevelGrid levelState, Color tint)
		{
			levelVisual.DrawLevelState(levelState, tint);
		}

		public void Print(string message, float size, TextAlignment alignment = TextAlignment.Left)
		{
			font.Print(message, size, alignment);
		}

		public void ResizeWindow(int width, int height)
		{
			levelVisual.ResizeWindow(width, height);
			font.ResizeWindow(width, height);
		}

		private VisualLevel levelVisual;
		private FontGL font;
	}
}