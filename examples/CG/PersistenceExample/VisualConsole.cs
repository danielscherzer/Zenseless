using System;

namespace Example
{
	public class VisualConsole
	{
		public static void DrawScreen(IGameState gameState)
		{
			Console.WriteLine("---------------------");
			for (int v = gameState.GridHeight - 1; v >= 0; --v)
			{
				string line = string.Empty;
				for (int u = 0; u < gameState.GridWidth; ++u)
				{
					switch (gameState[u, v])
					{
						case FieldType.DIAMONT:
							line += 'o';
							break;
						case FieldType.CROSS:
							line += 'x';
							break;
						default:
							line += '_';
							break;
					}
				}
				Console.WriteLine(line);
			}
		}
	}
}
