namespace Example
{
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	public class Model
	{
		public Model()
		{
			var random = new Random();
			for (int i = 0; i < 10; ++i)
			{
				units.Add(new Unit((float)random.NextDouble(), (float)random.NextDouble(), 0.05f, 0f));
			}
			units.Add(new Unit(0.5f, 0.5f, 0.05f, 180f));
			units.Add(new Unit(0.6f, 0.5f, 0.05f, 0f));
		}

		public IEnumerable<IUnit> Units => units;

		public void MoveTo(Vector2 destination)
		{
			var box = CalcSelectedUnitsBoundingBox();
			if (box is null) return;
			if (box.Contains(destination))
			{
				foreach (var unit in units)
				{
					if (unit.Selected)
					{
						unit.MoveTo(destination);
					}
				}
			}
			else
			{
				var dir = destination - box.GetCenter();
				foreach (var unit in units)
				{
					if (unit.Selected)
					{
						unit.MoveTo(unit.Bounds.Center + dir);
					}
				}
			}
		}

		public void Select(Vector2 selectionStart, Vector2 selectionEnd)
		{
			var selectionBox = Box2DExtensions.CreateFromPoints(new Vector2[] { selectionStart, selectionEnd });
			foreach(var unit in units)
			{
				var box = Box2DExtensions.CreateFromCenterSize(unit.Bounds.CenterX, unit.Bounds.CenterY
					, unit.Bounds.Radius * 2, unit.Bounds.Radius * 2);
				unit.Selected = selectionBox.Intersects(box);
			}
		}

		public void Update(float updatePeriod)
		{

			foreach (var unit in units)
			{
				unit.Update();
			}
			for (int i = 0; i < units.Count; ++i)
			{
				for (int j = i + 1; j < units.Count; ++j)
				{
					units[i].HandleCollision(units[j]);
				}
			}
		}

		private List<Unit> units = new List<Unit>();

		Box2D CalcSelectedUnitsBoundingBox()
		{
			Box2D mergedBox = null;
			foreach (var unit in units)
			{
				if (unit.Selected)
				{
					var box = Box2DExtensions.CreateFromCircle(unit.Bounds);
					if (mergedBox is null) mergedBox = box;
					else mergedBox = mergedBox.Merge(box);
				}
			}
			return mergedBox;
		}
	}
}