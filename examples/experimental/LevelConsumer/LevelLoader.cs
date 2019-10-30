using LevelData;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Zenseless.Patterns;

namespace Example
{
	static class LevelLoader
	{
		public static void LoadLevelData(string levelFile, GameLogic logic, SpriteRenderer renderer)
		{
			levelFile = Path.GetFullPath("level.data");
			if (!File.Exists(levelFile))
			{
				//find level editor
				var searchPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\.."));
				Console.WriteLine(searchPath);
				var editor = Directory.GetFiles(searchPath, "LevelEditor.exe", SearchOption.AllDirectories).FirstOrDefault();
				//create level data
				Process.Start(editor, $"autosave {levelFile}").WaitForExit();
			}
			using (var level = Serialization.FromBinFile(levelFile) as Level)
			{
				//set level bounds
				logic.Bounds = level.Bounds;
				//load colliders
				foreach (var collider in level.colliders)
				{
					logic.AddCollider(collider.Name, collider.Bounds);
				}
				//load sprites
				foreach (var sprite in level.Sprites)
				{
					renderer.AddSprite(sprite.Name, sprite.Layer, sprite.RenderBounds, sprite.NamedStream);
				}
			}
		}
	}
}
