using System;

namespace MvcSokoban
{
	[Serializable]
	public class Level : ILevel
	{
		public Level(int width, int height)
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

		private ElementType[,] arrTile;
	}
}
