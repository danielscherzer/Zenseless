namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Collections.Generic;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	internal class View
	{
		public View(IRenderState renderState)
		{
			renderState.Set(BlendStates.Additive);
		}

		internal void Render(IEnumerable<Collider> colliders)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.Color4(0.5f, 0.5f, 0.5f, 1f);
			GL.Begin(PrimitiveType.Quads);
			foreach (var collider in colliders)
			{
				DrawBox(collider.Box);
			}
			GL.End();
		}

		private static void DrawBox(IReadOnlyBox2D rect)
		{
			GL.Vertex2(rect.MinX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.MinX, rect.MaxY);
		}
	}
}