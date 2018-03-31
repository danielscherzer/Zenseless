namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Numerics;
	using Zenseless.HLGL;

	public class Visual
	{
		public Visual(IRenderState renderState, IContentLoader contentLoader)
		{
			shader = contentLoader.Load<IShaderProgram>("shader.*");
			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(BoolState<ILineSmoothState>.Enabled);
			GL.Enable(EnableCap.PointSmooth);
		}

		public void Render(IReadOnlyList<Vector2> points, int selectedPoint)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			//GL.LoadIdentity();
			//GL.Ortho(-windowAspect, windowAspect, -1, 1, 0, 1);

			shader.Activate();

			GL.LineWidth(3.0f);
			DrawLineStrip(points);
			shader.Deactivate();

			GL.Color3(Color.Red);
			GL.PointSize(15.0f);
			DrawPoints(points);

			if(-1 != selectedPoint)
			{
				GL.Color3(Color.Red);
				GL.PointSize(25.0f);
				DrawPoint(points[selectedPoint]);
			}
		}

		private readonly IShaderProgram shader;
		private float windowAspect;

		internal void Resize(int width, int height)
		{
			windowAspect = width / (float)height;
		}

		private void DrawPoint(Vector2 point)
		{
			GL.Begin(PrimitiveType.Points);
			GL.Vertex2(point.X, point.Y);
			GL.End();
		}

		private void DrawPoints(IEnumerable<Vector2> points)
		{
			GL.Begin(PrimitiveType.Points);
			foreach (var point in points)
			{
				GL.Vertex2(point.X, point.Y);
			}
			GL.End();
		}

		private void DrawLineStrip(IEnumerable<Vector2> points)
		{
			GL.Begin(PrimitiveType.LineStripAdjacency);
			var first = points.First();
			GL.Vertex2(first.X, first.Y);
			foreach (var point in points)
			{
				GL.Vertex2(point.X, point.Y);
			}
			var last = points.Last();
			GL.Vertex2(last.X, last.Y);
			GL.End();
		}
	}
}
