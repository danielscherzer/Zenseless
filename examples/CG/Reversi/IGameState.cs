namespace Reversi
{
	public enum FieldType { EMPTY, BLACK, WHITE };

	public interface IGameState
	{
		FieldType this[int x, int y] { get; }

		int GridHeight { get; }
		int GridWidth { get; }
		int LastMoveX { get; }
		int LastMoveY { get; }
	}
}