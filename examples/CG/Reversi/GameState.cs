using System;

namespace Reversi
{
	[Serializable]
	public class GameState : IGameState
	{
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

		public int LastMoveX { get; set; }
		public int LastMoveY { get; set; }

		private FieldType[,] grid = new FieldType[8, 8];
	}
}
