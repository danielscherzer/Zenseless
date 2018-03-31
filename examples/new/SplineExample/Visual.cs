namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class Visual
	{
		public Visual(IRenderState renderState)
		{
			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(BoolState<ILineSmoothState>.Enabled);
			GL.Enable(EnableCap.PointSmooth);
		}

		internal void Render(IReadOnlyList<Vector2> points, IReadOnlyList<Vector2> tangents, IReadOnlyList<Vector2> tangentHandles, int selectedPoint, int selectedTangentHandle)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.White);
			GL.LineWidth(3.0f);
			DrawSpline(points, tangents);

			GL.Color3(Color.Green);
			GL.LineWidth(2.0f);
			DrawLines(points, tangentHandles);

			GL.Color3(Color.Red);
			GL.PointSize(15.0f);
			DrawPoints(points);

			GL.Color3(Color.Green);
			GL.PointSize(8.0f);
			DrawPoints(tangentHandles);

			if(-1 != selectedPoint)
			{
				GL.Color3(Color.Red);
				GL.PointSize(25.0f);
				DrawPoint(points[selectedPoint]);
			}
			if (-1 != selectedTangentHandle)
			{
				GL.Color3(Color.Green);
				GL.PointSize(25.0f);
				DrawPoint(tangentHandles[selectedTangentHandle]);
			}
		}

		private void DrawPoint(Vector2 point)
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
			for (float t = 0; t <= points.Count - 1; t += 0.03f)
			{
				var activeSegment = CatmullRomSpline.FindSegment(t, points.Count);
				var pos = CatmullRomSpline.EvaluateSegment(points[activeSegment.Item1]
					, points[activeSegment.Item2]
					, tangents[activeSegment.Item1]
					, tangents[activeSegment.Item2]
					, t - (float)Math.Floor(t));

				GL.Vertex2(pos.X, pos.Y);
			}
			GL.End();
		}
	}
}
