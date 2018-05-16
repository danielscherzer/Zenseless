namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Collections.Generic;
	using Zenseless.Geometry;
	using Zenseless.OpenGL;

	internal class MyVisual
	{
		internal void Render(IEnumerable<IReadOnlyBox2D> shapes)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity(); // start with identity transformation
			GL.Scale(invWindowAspect, 1f, 1f); //then multiply by a scale matrix that scales all renderings with 1 / <window aspect>
			foreach (var box in shapes)
			{
				DrawTools.DrawTexturedRect(box, Box2D.BOX01);
			}
		}

		internal void Resize(int width, int height)
		{
			invWindowAspect = height / (float)width;
		}

		private float invWindowAspect;
	}
}