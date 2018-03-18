using System;
using System.IO;
using System.Linq;

namespace MvcSokoban
{
	public class LevelLoader
	{
		public static Level FromString(string levelString)
		{
			if (string.IsNullOrWhiteSpace(levelString)) return null;
			var lines = levelString.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
			int longestLength = lines.Max(s => s.Length);
			//use the longest line and line count as level dimensions
			Level level = new Level(longestLength, lines.Length);
			int y = level.Height - 1;
			foreach (string sLine in lines)
			{
				int x = 0;
				//each character is a grid element
				foreach (char symbol in sLine)
				{
					ElementType type = ElementType.Floor;
					switch (symbol)
					{
						case '#': type = ElementType.Wall; break;
						case '-': type = ElementType.Floor; break;
						case '@': type = ElementType.Man; break;
						case '$': type = ElementType.Box; break;
						case '.': type = ElementType.Goal; break;
						case '*': type = ElementType.BoxOnGoal; break;
						case '+': type = ElementType.ManOnGoal; break;
					};
					level.SetElement(x, y, type);
					++x;
				}
				--y;
			}
			return level;
		}

		public static Level FromFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException("Could not find level file '" + fileName + "'");
			}
			using (StreamReader sr = new StreamReader(fileName))
			{
				var levelString = sr.ReadToEnd();
				sr.Dispose();
				return FromString(levelString);
			}
		}
	}
}
