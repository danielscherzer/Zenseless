namespace Example
{
	public interface IGrid<ELEMENT>
	{
		int Height { get; }
		int Width { get; }

		ELEMENT GetElement(int x, int y);
	}
}