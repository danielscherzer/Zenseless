using LevelData;
using OpenTK.Input;
using System;
using Zenseless.ExampleFramework;
using Zenseless.Base;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var controller = new Controller();
			var logic = new GameLogic();
			var renderer = new SpriteRenderer(window.RenderContext.RenderState);
			logic.NewPosition += (name, x, y) => renderer.UpdateSprites(name, x, y);
			try
			{
				LoadLevelData("level.data", logic, renderer);
				window.Resize += (width, height) => renderer.Resize(width, height);
				window.Render += () => renderer.Render(logic.Bounds);
				window.Update += (updatePeriod) => HandleInput(updatePeriod, logic);
			}
			catch
			{
			}
			window.Run();

		}

		private static void LoadLevelData(string levelFile, GameLogic logic, SpriteRenderer renderer)
		{
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

		private static void HandleInput(float updatePeriod, GameLogic logic)
		{
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			float axisUpDown = Keyboard.GetState()[Key.Down] ? -1.0f : Keyboard.GetState()[Key.Up] ? 1.0f : 0.0f;
			logic.Update(updatePeriod, axisLeftRight, axisUpDown);
		}
	}
}