using System;
using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// In numerical analysis, a cubic Hermite spline or cubic Hermite interpolator is a spline where each piece 
	/// is a third-degree polynomial specified in Hermite form that is, by its values and first derivatives 
	/// at the end points of the corresponding domain interval (Wikipedia).
	/// </summary>
	public class CubicHermiteSpline
	{
		/// <summary>
		/// Evaluates a cubic Hermite spline segment given by two control points and tangents at parametric position t.
		/// </summary>
		/// <param name="point0">The first control point.</param>
		/// <param name="point1">The second control point.</param>
		/// <param name="tangent0">The tangent at the first control point.</param>
		/// <param name="tangent1">The tangent at the second control point.</param>
		/// <param name="t">The parametric position on the spline segment t e [0, 1].</param>
		/// <returns></returns>
		public static float EvaluateSegment(float point0, float point1, float tangent0, float tangent1, float t)
		{
			return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
		}

		/// <summary>
		/// Evaluates a cubic Hermite spline segment given by two control points and tangents at parametric position t.
		/// </summary>
		/// <param name="point0">The first control point.</param>
		/// <param name="point1">The second control point.</param>
		/// <param name="tangent0">The tangent at the first control point.</param>
		/// <param name="tangent1">The tangent at the second control point.</param>
		/// <param name="t">The parametric position on the spline segment t e [0, 1].</param>
		/// <returns></returns>
		public static Vector2 EvaluateSegment(Vector2 point0, Vector2 point1, Vector2 tangent0, Vector2 tangent1, float t)
		{
			return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
		}

		/// <summary>
		/// Evaluates a cubic Hermite spline segment given by two control points and tangents at parametric position t.
		/// </summary>
		/// <param name="point0">The first control point.</param>
		/// <param name="point1">The second control point.</param>
		/// <param name="tangent0">The tangent at the first control point.</param>
		/// <param name="tangent1">The tangent at the second control point.</param>
		/// <param name="t">The parametric position on the spline segment t e [0, 1].</param>
		/// <returns></returns>
		public static Vector3 EvaluateSegment(Vector3 point0, Vector3 point1, Vector3 tangent0, Vector3 tangent1, float t)
		{
			return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
		}

		/// <summary>
		/// Evaluates a cubic Hermite spline given by the specified points and tangents at parametric position t.
		/// </summary>
		/// <param name="points">The control points.</param>
		/// <param name="tangents">The tangents at the control points.</param>
		/// <param name="t">The parametric position on the spline t e [0, pointCount - 1].</param>
		/// <returns></returns>
		public static Vector2 EvaluateAt(IReadOnlyList<Vector2> points, IReadOnlyList<Vector2> tangents, float t)
		{
			t = MathHelper.Clamp(t, 0f, points.Count - 1);
			var id = MathHelper.FastTruncate(t);
			if (points.Count == id + 1) return points[points.Count - 1]; //t >= points.Count - 1, specifies exactly last control point
			var t01 = t - id;
			return EvaluateSegment(points[id], points[id + 1], tangents[id], tangents[id + 1], t01);
		}

		/// <summary>
		/// Calculates the position on the Catmull Rom Spline at parametric position t.
		/// </summary>
		/// <param name="points">The control points.</param>
		/// <param name="t">The parametric position on the spline t.</param>
		/// <returns>The position on the spline.</returns>
		public static Vector2 CatmullRomSpline(IReadOnlyList<Vector2> points, float t)
		{
			var segment = FindSegment(t, points.Count);
			var p0 = points[segment.Item1];
			var p1 = points[segment.Item2];
			var tangents = FinitDifferenceTangens(points, segment);
			return EvaluateSegment(p0, p1, tangents.Item1, tangents.Item2, t - (float)Math.Floor(t));
		}

		private static Tuple<Vector2, Vector2> FinitDifferenceTangens(IReadOnlyList<Vector2> points, Tuple<int, int> segment)
		{
			Vector2 FiniteDifference(Vector2 pointLeft, Vector2 pointRight) => 0.5f * (pointRight - pointLeft);

			var p0 = points[segment.Item1];
			var p1 = points[segment.Item2];
			var pBefore = 0 == segment.Item1 ? p0 : points[segment.Item1 - 1];
			var pAfter = points.Count == segment.Item2 + 1 ? p1 : points[segment.Item2 + 1];
			var t0 = FiniteDifference(pBefore, p1);
			var t1 = FiniteDifference(p0, pAfter);
			return new Tuple<Vector2, Vector2>(t0, t1);
		}


		/// <summary>
		/// Calculates the position on the Catmull Rom Spline loop at parametric position t.
		/// </summary>
		/// <param name="points">The control point loop.</param>
		/// <param name="t">The parametric position on the spline t.</param>
		/// <returns>The position on the spline.</returns>
		public static Vector2 CatmullRomSplineLoop(IReadOnlyList<Vector2> points, float t)
		{
			Tuple<int, int> FindSegmentLoop(int pointCount)
			{
				var id = (int)Math.Floor(t);
				return new Tuple<int, int>(id % pointCount, (id + 1) % pointCount);
			}
			var segment = FindSegmentLoop(points.Count);
			var p0 = points[segment.Item1];
			var p1 = points[segment.Item2];
			var pBefore = 0 == segment.Item1 ? points[points.Count - 1] : points[segment.Item1 - 1];
			var pAfter = points.Count == segment.Item2 + 1 ? points[0] : points[segment.Item2 + 1];
			var t0 = FiniteDifference(pBefore, p1);
			var t1 = FiniteDifference(p0, pAfter);
			return EvaluateSegment(p0, p1, t0, t1, t - (float)Math.Floor(t));
		}

		/// <summary>
		/// Finds the current segment (given by t) for a list of control points.
		/// </summary>
		/// <param name="t">The parameter t. Starts at 0 and increases by 1 for each control point.</param>
		/// <param name="pointCount">The point count.</param>
		/// <returns></returns>
		private static Tuple<int, int> FindSegment(float t, int pointCount)
		{
			var id = (int)Math.Floor(t) % pointCount;
			var id2 = Math.Min(id + 1, pointCount - 1);
			return new Tuple<int, int>(id, id2);
		}

		private static Vector2 FiniteDifference(Vector2 pointLeft, Vector2 pointRight) => 0.5f * (pointRight - pointLeft);
		private static float H1(float t) => 2 * t * t * t - 3 * t * t + 1;
		private static float H2(float t) => -2 * t * t * t + 3 * t * t;
		private static float H3(float t) => t * t * t - 2 * t * t + t;
		private static float H4(float t) => t * t * t - t * t;
	}
}
