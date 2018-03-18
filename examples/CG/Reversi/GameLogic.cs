using System;
using System.Drawing;

namespace Reversi
{
	public class GameLogic
	{
		public GameLogic()
		{
			for (int x = 0; x < grid.GridWidth; ++x)
			{
				for (int y = 0; y < grid.GridHeight; ++y)
				{
					grid[x, y] = FieldType.EMPTY;
				}
			}
			var centerX = (int)Math.Round(grid.GridWidth / 2f);
			var centerY = (int)Math.Round(grid.GridHeight / 2f);
			grid[centerX - 1, centerY - 1] = FieldType.BLACK;
			grid[centerX - 1, centerY] = FieldType.WHITE;
			grid[centerX, centerY - 1] = FieldType.WHITE;
			grid[centerX, centerY] = FieldType.BLACK;
			grid.LastMove = new Point(centerX, centerY - 1);
			UpdateGameResult();
		}

		public int CountWhite { get; private set; }
		public int CountBlack { get; private set; }
		public enum Result { PLAYING, BLACK_WON, WHITE_WON, DRAW };
		public Result CurrentGameResult { get; private set; }
		public IGameState GameState { get { return grid; } }

		public void Move(Point point)
		{
			var x = point.X;
			var y = point.Y;
			if (x < 0 || grid.GridWidth <= x) return;
			if (y < 0 || grid.GridHeight <= y) return;
			if (FieldType.EMPTY != grid[x, y]) return;
			var color = whiteMoves ? FieldType.WHITE : FieldType.BLACK;
			grid[x, y] = color;
			grid.LastMove = new Point(x, y);
			for (int dirX = -1; dirX <= 1; ++dirX)
			{
				for (int dirY = -1; dirY <= 1; ++dirY)
				{
					if (0 == dirX && 0 == dirY) continue;
					Reverse(x, y, color, dirX, dirY);
				}
			}
			whiteMoves = !whiteMoves;
			UpdateGameResult();
		}

		private GameState grid = new GameState();
		private bool whiteMoves = false;

		private void Reverse(int startX, int startY, FieldType fillColor, int dirX, int dirY)
		{
			var otherColor = FieldType.BLACK == fillColor ? FieldType.WHITE : FieldType.BLACK;
			//search how many to reverse
			for (int i = 1; true; ++i)
			{
				//go one step into direction
				int x = startX + i * dirX;
				int y = startY + i * dirY;
				//check out of bounds
				if (x < 0 || grid.GridWidth <= x) return;
				if (y < 0 || grid.GridHeight <= y) return;
				if (otherColor != grid[x, y])
				{
					if (fillColor == grid[x, y])
					{
						for (int j = 1; j < i; ++j)
						{
							//reverse
							int reverseX = startX + j * dirX;
							int reverseY = startY + j * dirY;
							grid[reverseX, reverseY] = fillColor;
						}
					}
					return;
				}
			}
		}

		private void UpdateGameResult()
		{
			CountWhite = 0;
			CountBlack = 0;
			for (int x = 0; x < grid.GridWidth; ++x)
			{
				for (int y = 0; y < grid.GridHeight; ++y)
				{
					switch (grid[x, y])
					{
						case FieldType.BLACK: ++CountBlack; break;
						case FieldType.WHITE: ++CountWhite; break;
					}
				}
			}

			if (grid.GridWidth * grid.GridHeight == CountWhite + CountBlack)
			{
				if (CountWhite == CountBlack)
				{
					CurrentGameResult = Result.DRAW;
				}
				else
				{
					CurrentGameResult = CountWhite > CountBlack ? Result.WHITE_WON : Result.BLACK_WON;
				}
			}
			else
			{
				CurrentGameResult = Result.PLAYING;
			}
		}
	}
}
