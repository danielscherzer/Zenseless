namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	internal class Model
	{
		public Model()
		{
			Add(Vector2.Zero);
			Add(new Vector2(0.5f, 0.0f));
		}

		public int SelectedPoint { get; private set; } = -1;
		public int SelectedTangent { get; private set; } = -1;
		public IReadOnlyList<Vector2> Points { get => points; }
		public IReadOnlyList<Vector2> Tangents { get => tangents; }
		public IReadOnlyList<Vector2> TangentHandles { get => tangentHandles; }

		internal void BeginEdit(in Vector2 coord)
		{
			if (-1 != SelectedTangent)
			{
				state = State.MoveTangent;
				return;
			}
			if (-1 != SelectedPoint)
			{
				state = State.MovePoint;
				return;
			}
			Add(coord);
			SelectedTangent = tangents.Count - 1;
			state = State.MoveTangent;
		}

		private void Add(in Vector2 coord)
		{
			points.Add(coord);
			var tangentHandle = coord + Vector2.One * 0.1f;
			tangentHandles.Add(tangentHandle);
			tangents.Add(tangentHandle - coord);
		}

		internal void Delete(in Vector2 coord)
		{
			if (-1 != SelectedPoint)
			{
				points.RemoveAt(SelectedPoint);
				tangents.RemoveAt(SelectedPoint);
				tangentHandles.RemoveAt(SelectedPoint);
				SelectedTangent = -1;
				SelectedPoint = -1;
			}
		}

		internal void EndEdit()
		{
			state = State.Default;
		}

		private enum State
		{
			Default, MovePoint, MoveTangent
		}

		private State state = State.Default;
		private List<Vector2> points = new List<Vector2>();
		private List<Vector2> tangentHandles = new List<Vector2>();
		private List<Vector2> tangents = new List<Vector2>();

		internal void Move(in Vector2 coord)
		{
			switch(state)
			{
				case State.MovePoint:
					points[SelectedPoint] = coord;
					break;
				case State.MoveTangent:
					tangentHandles[SelectedTangent] = coord;
					var diff = coord - points[SelectedTangent];
					tangents[SelectedTangent] = diff;
					break;
				default:
					SelectedPoint = -1;
					SelectedTangent = -1;
					var selectionCircle = new Circle(coord.X, coord.Y, 0.03f);
					for (int i = 0; i < points.Count; ++i)
					{
						if (selectionCircle.Contains(points[i]))
						{
							SelectedPoint = i;
							break;
						}
					}
					for (int i = 0; i < tangentHandles.Count; ++i)
					{
						if (selectionCircle.Contains(tangentHandles[i]))
						{
							SelectedTangent = i;
							break;
						}
					}
					break;
			}
		}
	}
}