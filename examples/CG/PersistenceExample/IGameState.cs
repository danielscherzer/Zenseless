namespace Example
{
	public enum FieldType { EMPTY, CROSS, DIAMONT };

	public interface IGameState
	{
		FieldType this[int x, int y] { get; }

		int GridHeight { get; }
		int GridWidth { get; }
	}
}