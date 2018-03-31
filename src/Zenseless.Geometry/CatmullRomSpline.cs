using System;
using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public class CatmullRomSpline
	{
		/// <summary>
		/// H1s the specified t.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static float H1(float t)
		{
			return 2 * t * t * t - 3 * t * t + 1;
		}

		/// <summary>
		/// H2s the specified t.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static float H2(float t)
		{
			return -2 * t * t * t + 3 * t * t;
		}

		/// <summary>
		/// H3s the specified t.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static float H3(float t)
		{
			return t * t * t - 2 * t * t + t;
		}

		/// <summary>
		/// H4s the specified t.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static float H4(float t)
		{
			return t * t * t - t * t;
		}

		/// <summary>
		/// Evaluates the segment.
		/// </summary>
		/// <param name="point0">The point0.</param>
		/// <param name="point1">The point1.</param>
		/// <param name="tangent0">The tangent0.</param>
		/// <param name="tangent1">The tangent1.</param>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static float EvaluateSegment(float point0, float point1, float tangent0, float tangent1, float t)
		{
			return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
		}

		/// <summary>
		/// Evaluates the segment.
		/// </summary>
		/// <param name="point0">The point0.</param>
		/// <param name="point1">The point1.</param>
		/// <param name="tangent0">The tangent0.</param>
		/// <param name="tangent1">The tangent1.</param>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static Vector2 EvaluateSegment(Vector2 point0, Vector2 point1, Vector2 tangent0, Vector2 tangent1, float t)
		{
			return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
		}

		/// <summary>
		/// Evaluates the segment.
		/// </summary>
		/// <param name="point0">The point0.</param>
		/// <param name="point1">The point1.</param>
		/// <param name="tangent0">The tangent0.</param>
		/// <param name="tangent1">The tangent1.</param>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static Vector3 EvaluateSegment(Vector3 point0, Vector3 point1, Vector3 tangent0, Vector3 tangent1, float t)
		{
			return H1(t) * point0 + H2(t) * point1 + H3(t) * tangent0 + H4(t) * tangent1;
		}

		/// <summary>
		/// Finds the segment for a control point loop.
		/// </summary>
		/// <param name="t">The parameter t. Starts at 0 and increases by 1 for each control point.</param>
		/// <param name="pointCount">The point count.</param>
		/// <returns></returns>
		public static Tuple<int, int> FindSegmentLoop(float t, int pointCount)
		{
			var id = (int)Math.Floor(t);
			return new Tuple<int, int>(id % pointCount, (id + 1) % pointCount);
		}

		/// <summary>
		/// Finds the current segment (given by t) for a list of control points.
		/// </summary>
		/// <param name="t">The parameter t. Starts at 0 and increases by 1 for each control point.</param>
		/// <param name="pointCount">The point count.</param>
		/// <returns></returns>
		public static Tuple<int, int> FindSegment(float t, int pointCount)
		{
			var id = (int)Math.Floor(t);
			var id2 = Math.Min(id + 1, pointCount - 1);
			return new Tuple<int, int>(id, id2);
		}

		/// <summary>
		/// Calculate the finite difference for each point.
		/// </summary>
		/// <param name="points">The input points</param>
		/// <returns></returns>
		public static List<Vector2> FiniteDifference(IList<Vector2> points)
		{
			var output = new List<Vector2>();
			if (points.Count < 2)
			{
				if (points.Count == 1) output.Add(points[0]);
				return output;
			}
			//first tangent
			output.Add(FiniteDifference(points[0], points[1]));
			//the rest except last
			for (int i = 0; i < points.Count - 2; ++i)
			{
				output.Add(FiniteDifference(points[i], points[i + 2]));
			}
			//add last
			output.Add(FiniteDifference(points[points.Count - 2], points[points.Count - 1]));
			return output;
		}

		/// <summary>
		/// Finites the difference loop.
		/// </summary>
		/// <param name="points">The points.</param>
		/// <returns></returns>
		public static List<Vector2> FiniteDifferenceLoop(IList<Vector2> points)
		{
			var output = new List<Vector2>();
			if (points.Count < 2) return output;
			//first tangent
			output.Add(FiniteDifference(points[points.Count - 1], points[1]));
			//the rest except last
			for (int i = 0; i < points.Count - 2; ++i)
			{
				output.Add(FiniteDifference(points[i], points[i + 2]));
			}
			//add last
			output.Add(FiniteDifference(points[points.Count - 2], points[0]));
			return output;
		}

		/// <summary>
		/// Finites the difference loop.
		/// </summary>
		/// <param name="points">The points.</param>
		/// <returns></returns>
		public static List<Vector3> FiniteDifferenceLoop(IList<Vector3> points)
		{
			var output = new List<Vector3>();
			if (points.Count < 3) return output;
			//first tangent
			output.Add(FiniteDifference(points[points.Count - 1], points[1]));
			//the rest except last
			for (int i = 0; i < points.Count - 2; ++i)
			{
				output.Add(FiniteDifference(points[i], points[i + 2]));
			}
			//add last
			output.Add(FiniteDifference(points[points.Count - 2], points[0]));
			return output;
		}

		private static Vector2 FiniteDifference(Vector2 pointLeft, Vector2 pointRight) => 0.5f * (pointRight - pointLeft);
		private static Vector3 FiniteDifference(Vector3 pointLeft, Vector3 pointRight) => 0.5f * (pointRight - pointLeft);
	}
}
