using System.Drawing;

namespace MvcSokoban
{
	public static class LevelHelper
	{
		public static Level Copy(this ILevel level)
		{
			var copy = new Level(level.Width, level.Height);
			for(int x = 0; x < level.Width; ++x)
			{
				for (int y = 0; y < level.Height; ++y)
				{
					copy.SetElement(x, y, level.GetElement(x, y));
				}
			}
			return copy;
		}

		public static Point Move(this Point pos, Movement movement)
		{
			switch (movement)
			{
				case Movement.DOWN: return new Point(pos.X, pos.Y - 1);
				case Movement.UP: return new Point(pos.X, pos.Y + 1);
				case Movement.LEFT: return new Point(pos.X - 1, pos.Y);
				case Movement.RIGHT: return new Point(pos.X + 1, pos.Y);
				default: return pos;
			}
		}

		public static ElementType GetElement(this ILevel level, Point position)
		{
			return level.GetElement(position.X, position.Y);
		}

		public static bool ContainsBox(this ElementType type)
		{
			return 0 != (type & ElementType.Box);
		}

		//public static bool ContainsGoal(this ElementType type)
		//{
		//	return 0 != (type & ElementType.Goal);
		//}

		public static bool ContainsMan(this ElementType type)
		{
			return 0 != (type & ElementType.Man);
		}

		public static void SetElement(this Level level, Point position, ElementType type)
		{
			level.SetElement(position.X, position.Y, type);
		}

		public static void MoveBox(this Level level, Point oldPos, Point newPos)
		{
			var type = level.GetElement(oldPos);
			switch (type)
			{
				case ElementType.Box: level.SetElement(oldPos, ElementType.Floor); break;
				case ElementType.BoxOnGoal: level.SetElement(oldPos, ElementType.Goal); break;
				default: return;
			}
			var type2 = level.GetElement(newPos);
			switch (type2)
			{
				case ElementType.Floor: level.SetElement(newPos, ElementType.Box); break;
				case ElementType.Goal: level.SetElement(newPos, ElementType.BoxOnGoal); break;
				default: return;
			}
		}

		public static void MovePlayer(this Level level, Point oldPos, Point newPos)
		{
			ElementType type = level.GetElement(oldPos);
			switch (type)
			{
				case ElementType.Man: level.SetElement(oldPos, ElementType.Floor); break;
				case ElementType.ManOnGoal: level.SetElement(oldPos, ElementType.Goal); break;
				default: return;
			}
			ElementType type2 = level.GetElement(newPos);
			switch (type2)
			{
				case ElementType.Floor: level.SetElement(newPos, ElementType.Man); break;
				case ElementType.Goal: level.SetElement(newPos, ElementType.ManOnGoal); break;
				default: return;
			}
		}

		public static bool IsEmpty(this ILevel level)
		{
			for (int x = 0; x < level.Width; ++x)
			{
				for (int y = 0; y < level.Height; ++y)
				{
					if (level.GetElement(x, y).ContainsBox()) return false;
				}
			}
			return true;
		}

		public static Point? FindPlayerPos(this ILevel level)
		{
			for (int x = 0; x < level.Width; ++x)
			{
				for (int y = 0; y < level.Height; ++y)
				{
					var type = level.GetElement(x, y);
					if (type.ContainsMan())
					{
						return new Point(x, y);
					}
				}
			}
			return null;
		}

		public static bool IsWon(this ILevel level)
		{
			for (int x = 0; x < level.Width; ++x)
			{
				for (int y = 0; y < level.Height; ++y)
				{
					var type = level.GetElement(x, y);
					//if a single goal without a box is found the game is not yet won.
					if (ElementType.Goal == type || ElementType.ManOnGoal == type)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
