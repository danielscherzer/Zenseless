using OpenTK.Graphics.OpenGL4;
using Zenseless.HLGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IContentLoader contentLoader)
		{
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
		}

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shaderProgram.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
	}
}