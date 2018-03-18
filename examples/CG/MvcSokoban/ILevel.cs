namespace MvcSokoban
{
	public enum ElementType { Floor = 0b0, Wall = 0b10, Man = 0b100, Box = 0b1000, Goal = 0b10000, BoxOnGoal = 0b11000, ManOnGoal = 0b10100 };

	public interface ILevel
	{
		int Height { get; }
		int Width { get; }

		ElementType GetElement(int x, int y);
	}
}