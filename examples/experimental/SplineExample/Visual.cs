namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class Visual
	{
		private readonly IRenderState renderState;

		public Visual(IRenderState renderState)
		{
			this.renderState = renderState ?? throw new ArgumentNullException(nameof(renderState));
			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(new LineSmoothing(true));
			GL.Enable(EnableCap.PointSmooth);
		}

		internal void Render(IReadOnlyList<Vector2> points, IReadOnlyList<Vector2> tangents, IReadOnlyList<Vector2> tangentHandles, int selectedPoint, int selectedTangentHandle)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(OpenTK.Color.White);
			renderState.Set(new LineWidth(3f));
			DrawSpline(points, tangents);

			GL.Color3(OpenTK.Color.Green);
			renderState.Set(new LineWidth(2f));
			DrawLines(points, tangentHandles);

			GL.Color3(OpenTK.Color.Red);
			GL.PointSize(15.0f);
			DrawPoints(points);

			GL.Color3(OpenTK.Color.Green);
			GL.PointSize(8.0f);
			DrawPoints(tangentHandles);

			if(-1 != selectedPoint)
			{
				GL.Color3(OpenTK.Color.Red);
				GL.PointSize(25.0f);
				DrawPoint(points[selectedPoint]);
			}
			if (-1 != selectedTangentHandle)
			{
				GL.Color3(OpenTK.Color.Green);
				GL.PointSize(25.0f);
				DrawPoint(tangentHandles[selectedTangentHandle]);
			}
		}

		private void DrawPoint(in Vector2 point)
		{
			GL.Begin(PrimitiveType.Points);
			GL.Vertex2(point.X, point.Y);
			GL.End();
		}

		private void DrawLines(IEnumerable<Vector2> a, IEnumerable<Vector2> b)
		{
			GL.Begin(PrimitiveType.Lines);
			using (IEnumerator<Vector2>	e1 = a.GetEnumerator(), e2 = b.GetEnumerator())
			{
				while (e1.MoveNext() && e2.MoveNext())
				{
					GL.Vertex2(e1.Current.X, e1.Current.Y);
					GL.Vertex2(e2.Current.X, e2.Current.Y);
				}
			}
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

		private void DrawSpline(IReadOnlyList<Vector2> points, IReadOnlyList<Vector2> tangents)
		{
			if (points.Count < 2) return;
			GL.Begin(PrimitiveType.LineStrip);
			var delta = 0.03f;
			for (float t = 0; t <= points.Count - 1 + delta; t += delta)
			{
				var pos = CubicHermiteSpline.EvaluateAt(points, tangents, t);
				GL.Vertex2(pos.X, pos.Y);
			}
			GL.End();
		}
	}
}
