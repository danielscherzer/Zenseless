using OpenTK.Graphics.OpenGL;
using Zenseless.HLGL;

namespace Example
{
	public class Canvas
	{
		public Canvas(IRenderState renderState)
		{
			renderState.Set(new ClearColorState(1f, 1f, 1f, 1f));
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.PolygonSmooth); //TODO: does not work on intel -> multi sample
		}

		public void Draw()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.Color3((byte)55, (byte)96, (byte)146);
			GL.Begin(PrimitiveType.Triangles);
			GL.Vertex2(-0.7, 0.7);
			GL.Vertex2(0.7, 0.2);
			GL.Vertex2(0, -0.7);
			GL.End();
		}
	}
}
