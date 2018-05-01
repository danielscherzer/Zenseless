namespace Example
{
	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL;
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	class MyVisual
	{
		private MyVisual(IRenderState renderState)
		{
			//background clear color
			renderState.Set(new ClearColorState(1, 1, 1, 1));
			GL.AlphaFunc(AlphaFunction.Less, 0.25f);
			GL.Enable(EnableCap.AlphaTest);
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			var rect = new Box2D(-.75f, -.75f, 1.5f, 1.5f);
			var colorA = new Color4(1f, 1f, 0f, 1f);
			var colorB = new Color4(0f, 1f, 1f, 0f);

			DrawRect(rect, colorA, colorB);
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual(window.RenderContext.RenderState);
			window.Render += visual.Render;
			window.Run();
		}

		private void DrawRect(IReadOnlyBox2D rectangle, Color4 colorA, Color4 colorB)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Color4(colorA);
			GL.Vertex2(rectangle.MinX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MinY);
			GL.Color4(colorB);
			GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.Vertex2(rectangle.MinX, rectangle.MaxY);
			GL.End();
		}
	}
}