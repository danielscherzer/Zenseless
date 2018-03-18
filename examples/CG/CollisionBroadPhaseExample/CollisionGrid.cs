using Zenseless.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Example
{
	public class CollisionGrid
	{
		public delegate void CollisionHandler(IBox2DCollider a, IBox2DCollider b);

		public Box2D Bounds { get; private set; }
		public Vector2 CellSize { get; private set; }

		public readonly int CellCountX;
		public readonly int CellCountY;

		public CollisionGrid(IReadOnlyBox2D bounds, float cellSizeX, float cellSizeY)
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

		public void Insert(IBox2DCollider objectBounds)
		{
			// Convert the object's AABB to integer grid coordinates.
			// Objects outside of the grid are clamped to the edge.
			int minX = Math.Max((int)Math.Floor((objectBounds.MinX - Bounds.MinX) / CellSize.X), 0);
			int maxX = Math.Min((int)Math.Floor((objectBounds.MaxX - Bounds.MinX) / CellSize.X), CellCountX - 1);
			int minY = Math.Max((int)Math.Floor((objectBounds.MinY - Bounds.MinY) / CellSize.Y), 0);
			int maxY = Math.Min((int)Math.Floor((objectBounds.MaxY - Bounds.MinY) / CellSize.Y), CellCountY - 1);

			// Loop over the cells the object overlaps and insert the object.
			for (int y = minY; y <= maxY; ++y)
			{
				for (int x = minX; x <= maxX; ++x)
				{
					cells[x, y].Add(objectBounds); 
				}
			}
		}

		public void Clear()
		{
			foreach(var cell in cells)
			{
				cell.Clear();
			}
		}

		public void FindAllCollisions(IEnumerable<IBox2DCollider> colliders, CollisionHandler Handler)
		{
			if (ReferenceEquals(null, Handler)) return;
			Clear();
			foreach (var collider in colliders)
			{
				Insert(collider);
			}
			for (int y = 0; y < CellCountY; ++y)
			{
				for (int x = 0; x < CellCountX; ++x)
				{
					var cell = cells[x, y];
					for(int i = 0; i < cell.Count; ++i)
					{
						for (int j = i + 1; j < cell.Count; ++j)
						{
							Handler(cell[i], cell[j]);
						}
					}
				}
			}
		}

		public IEnumerable<IBox2DCollider> this[int x, int y] { get { return cells[x, y]; } }

		private List<IBox2DCollider>[,] cells;
	}
}
