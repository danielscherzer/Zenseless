using System;

namespace Example
{
	[Serializable]
	public class LevelGrid : ILevelGrid
	{
		public LevelGrid(int width, int height)
		{
			this.Width = width;
			this.Height = height;
			arrTile = new ElementType[Width, Height];
		}

		public ElementType GetElement(int x, int y)
		{
			return arrTile[x, y];
		}

		public void SetElement(int x, int y, ElementType value)
		{
			arrTile[x, y] = value;
		}

		public int Height { get; private set; }

		public int Width { get; private set; }

		private readonly ElementType[,] arrTile;
	}
}
