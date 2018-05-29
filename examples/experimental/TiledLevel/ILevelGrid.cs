namespace Example
{
	public interface ILevelGrid
	{
		int Height { get; }
		int Width { get; }

		byte GetElement(int x, int y);
	}
}