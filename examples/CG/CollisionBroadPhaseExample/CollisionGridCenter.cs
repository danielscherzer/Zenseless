namespace Example
{
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;

	public class CollisionGridCenter
	{
		public CollisionGridCenter(IReadOnlyBox2D bounds, float cellSizeX, float cellSizeY)
		{
			Bounds = new Box2D(bounds);
			CellSize = new Vector2(cellSizeX, cellSizeY);
			CellCountX = (int)(bounds.SizeX / CellSize.X);
			CellCountY = (int)(bounds.SizeY / CellSize.Y);

			cells = new List<IBox2DCollider>[CellCountX, CellCountY];
			for (int y = 0; y < CellCountY; ++y)
			{
				for (int x = 0; x < CellCountX; ++x)
				{
					cells[x, y] = new List<IBox2DCollider>();
				}
			}
		}

		public delegate void CollisionHandler(IBox2DCollider a, IBox2DCollider b);

		public Box2D Bounds { get; private set; }
		public Vector2 CellSize { get; private set; }

		public readonly int CellCountX;
		public readonly int CellCountY;

		public void Clear()
		{
			foreach (var cell in cells)
			{
				cell.Clear();
			}
		}


		public void FindAllCollisions(IEnumerable<IBox2DCollider> colliders, CollisionHandler Handler)
		{
			if (Handler is null) return;
			Clear();
			foreach (var collider in colliders)
			{
				Insert(collider);
			}
			for (int y = 0; y < CellCountY - 1; ++y)
			{
				for (int x = 0; x < CellCountX - 1; ++x)
				{
					var cell = cells[x, y];
					CheckCell(Handler, cell);
					//check all bigger neighbor cells (others are tested from different cells)
					CheckCells(Handler, cell, cells[x + 1, y]);
					CheckCells(Handler, cell, cells[x, y + 1]);
					CheckCells(Handler, cell, cells[x + 1, y + 1]);
				}
			}
			// biggest x column
			for (int y = 0; y < CellCountY - 1; ++y)
			{
				var cell = cells[CellCountX - 1, y];
				CheckCell(Handler, cell);
				//check all bigger neighbor cells (others are tested from different cells)
				CheckCells(Handler, cell, cells[CellCountX - 1, y + 1]);
			}
			//biggest y-row
			for (int x = 0; x < CellCountX - 1; ++x)
			{
				var cell = cells[x, CellCountY - 1];
				CheckCell(Handler, cell);
				//check all bigger neighbor cells (others are tested from different cells)
				CheckCells(Handler, cell, cells[x + 1, CellCountY - 1]);
			}
			//corner cell with itself
			CheckCell(Handler, cells[CellCountX - 1, CellCountY - 1]);
		}

		private void CheckCells(CollisionHandler Handler, List<IBox2DCollider> cellA, List<IBox2DCollider> cellB)
		{
			foreach(var colliderA in cellA)
			{
				foreach (var colliderB in cellB)
				{
					Handler(colliderA, colliderB);
				}
			}
		}

		private static void CheckCell(CollisionHandler Handler, List<IBox2DCollider> cell)
		{
			for (int i = 0; i < cell.Count; ++i)
			{
				//check each collider against every other collider
				for (int j = i + 1; j < cell.Count; ++j)
				{
					Handler(cell[i], cell[j]);
				}
			}
		}

		public void Insert(IBox2DCollider objectBounds)
		{
			//var cX = objectBounds.MinX;
			//var cY = objectBounds.MinY;
			var cX = objectBounds.MinX + 0.5f * (objectBounds.MaxX - objectBounds.MinX);
			var cY = objectBounds.MinY + 0.5f * (objectBounds.MaxY - objectBounds.MinY);
			// Convert to integer grid coordinates.
			int minX = Math.Max((int)Math.Floor((cX - Bounds.MinX) / CellSize.X), 0);
			int minY = Math.Max((int)Math.Floor((cY - Bounds.MinY) / CellSize.Y), 0);

			cells[minX, minY].Add(objectBounds);
		}

		private readonly List<IBox2DCollider>[,] cells;
	}
}
