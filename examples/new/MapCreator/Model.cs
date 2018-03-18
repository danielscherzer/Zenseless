namespace Example
{
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	internal class Model
	{
		public Model()
		{
			random = new Random();
			CreatePath();
		}

		public IEnumerable<IEnumerable<Vector2>> Paths { get => paths; }
		public IEnumerable<Vector2> Points { get; private set; }

		public void CreatePath()
		{
			paths.Clear();
			var points = new List<Vector2>() { Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY };
			points.Add(new Vector2((float)random.NextDouble(), (float)random.NextDouble()) * 0.7f + new Vector2(0.3f));

			for(int i = 0; i + 1 < points.Count; ++i)
			{
				for (int j = i + 1; j < points.Count; ++j)
				{
					paths.Add(MovingAverage(CreatePath(points[i], points[j]), 3));
				}
			}
			Points = points;
		}

		private List<Vector2> MovingAverage(IReadOnlyList<Vector2> path, int halfKernel)
		{
			var result = new List<Vector2>() { path[0] };
			for (int i = 0; i + halfKernel - 1 < path.Count; ++i)
			{
				var value = Vector2.Zero;
				for(int j = -halfKernel; j <= halfKernel; ++j)
				{
					var index = (i + j).Clamp(0, path.Count - 1);
					value += path[index];
				}
				value /= 2 * halfKernel + 1;
				result.Add(value);
			}
			result.Add(path[path.Count - 1]);
			return result;
		}

		private float Length(List<Vector2> path)
		{
			var length = 0f;
			for (int i = 0; i + 1 < path.Count; ++i)
			{
				var a = path[i];
				var b = path[i + 1];
				length += Vector2.Distance(a, b);
			}
			return length;
		}

		public List<Vector2> CreatePath(Vector2 a, Vector2 b)
		{
			var result = new List<Vector2> { a, b };
			for (var i = 0; i < 7; ++i) result = Split(result);
			return result;
		}

		private List<Vector2> Split(List<Vector2> points)
		{
			var result = new List<Vector2> { points[0] };
			for (int i = 0; i + 1 < points.Count; ++i)
			{
				var a = points[i];
				var b = points[i + 1];
				var middle = 0.5f * (a + b);
				var normal = (middle - a).CcwNormalTo();
				var delta = 2 * (float)random.NextDouble() - 1;
				var point = middle + delta * normal;
				point = MathHelper.Clamp(point, 0, 1);
				result.Add(point);
				result.Add(b);
			}
			return result;
		}

		private List<List<Vector2>> paths = new List<List<Vector2>>();
		private readonly Random random;
	}
}