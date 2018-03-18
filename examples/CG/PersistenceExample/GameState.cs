using System;

namespace Example
{
	[Serializable]
	public class GameState : IGameState
	{
		private FieldType[,] grid = new FieldType[8, 8];

		public GameState()
		{
			for (int x = 0; x < GridWidth; ++x)
			{
				for (int y = 0; y < GridHeight; ++y)
				{
					grid[x, y] = FieldType.EMPTY;
				}
			}
		}

		public FieldType this[int x, int y]
		{
			get { return grid[x, y]; }
			set { grid[x, y] = value; }
		}

		public int GridWidth { get { return grid.GetLength(0); } }
		public int GridHeight { get { return grid.GetLength(1); } }
	}
}
