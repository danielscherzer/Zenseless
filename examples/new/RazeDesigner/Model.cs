namespace Example
{
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	internal class Model
	{
		public Model()
		{
			Add(new Vector2(-0.5f, -0.5f));
			Add(Vector2.Zero);
			Add(new Vector2(0.5f, 0.0f));
		}

		public int SelectedPoint { get; private set; } = -1;
		public IReadOnlyList<Vector2> Points { get => points; }

		internal void BeginEdit(Vector2 coord)
		{
			if (-1 != SelectedPoint)
			{
				state = State.MovePoint;
				return;
			}
			Add(coord);
		}

		private void Add(Vector2 coord)
		{
			points.Add(coord);
		}

		internal void Delete(Vector2 coord)
		{
			if (-1 != SelectedPoint)
			{
				points.RemoveAt(SelectedPoint);
				SelectedPoint = -1;
			}
		}

		internal void EndEdit()
		{
			state = State.Default;
		}

		private enum State
		{
			Default, MovePoint
		}

		private State state = State.Default;
		private List<Vector2> points = new List<Vector2>();

		internal void Move(Vector2 coord)
		{
			switch(state)
			{
				case State.MovePoint:
					points[SelectedPoint] = coord;
					break;
				default:
					SelectedPoint = -1;
					var selectionCircle = new Circle(coord.X, coord.Y, 0.06f);
					for (int i = 0; i < points.Count; ++i)
					{
						if (selectionCircle.Contains(points[i]))
						{
							SelectedPoint = i;
							break;
						}
					}
					break;
			}
		}
	}
}