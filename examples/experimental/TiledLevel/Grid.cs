namespace Example
{
	public class Grid<ELEMENT>
	{
		public Grid(int width, int height)
		{
			Width = width;
			Height = height;
			arrTile = new ELEMENT[Width, Height];
		}

		public void Clear(ELEMENT value)
		{
			for(int x = 0; x < Width; ++x)
			{
				for (int y = 0; y < Height; ++y)
				{
					arrTile[x, y] = value;
				}
			}
		}

		public ELEMENT GetElement(int x, int y)
		{
			return arrTile[x, y];
		}

		public void SetElement(int x, int y, ELEMENT value)
		{
			arrTile[x, y] = value;
		}

		public int Height { get; private set; }

		public int Width { get; private set; }

		private readonly ELEMENT[,] arrTile;
	}
}
