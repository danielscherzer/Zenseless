using Zenseless.ExampleFramework;
using Zenseless.Geometry;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	class MyVisual
	{
		private MyVisual(IRenderState renderState)
		{
			//background clear color
			renderState.Set(new ClearColorState(1, 1, 1, 1));
			//setup blending equation Color = Color_new · alpha + Color_before · (1 - alpha)
			renderState.Set(new BlendState(BlendOperator.Add, BlendParameter.SourceAlpha, BlendParameter.OneMinusSourceAlpha));
			//renderState.Set(BlendStates.AlphaBlend); // does the same
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			var rect = new Box2D(-.75f, -.75f, .5f, .5f);
			DrawRect(rect, new Color4(.5f, .7f, .1f, 1));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.7f, .5f, .9f, .5f));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.7f, .5f, .9f, .5f));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.5f, .7f, 1, .5f));
			rect.MinX += .25f;
			rect.MinY += .25f;
			DrawRect(rect, new Color4(.5f, .7f, 1, .5f));
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual(window.RenderContext.RenderState);
			window.Render += visual.Render;
			window.Run();
		}

		private void DrawRect(IReadOnlyBox2D rectangle, Color4 color)
		{
			GL.Color4(color);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(rectangle.MinX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.Vertex2(rectangle.MinX, rectangle.MaxY);
			GL.End();
		}

		public void Update()
		{
			Render();
		}
	}
}