using System;

namespace Example
{
	[Serializable]
	public class LevelGrid : ILevelGrid
	{
		public LevelGrid(int width, int height)
		{
			Width = width;
			Height = height;
			arrTile = new byte[Width, Height];
		}

		public byte GetElement(int x, int y)
		{
			return arrTile[x, y];
		}

		public void SetElement(int x, int y, byte value)
		{
			arrTile[x, y] = value;
		}

		public int Height { get; private set; }

		public int Width { get; private set; }

		private readonly byte[,] arrTile;
	}
}
