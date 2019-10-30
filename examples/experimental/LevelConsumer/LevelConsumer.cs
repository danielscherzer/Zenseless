using System;
using Zenseless.ExampleFramework;
using Zenseless.HLGL;

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
			LevelLoader.LoadLevelData("level.data", logic, renderer);
			window.Resize += (width, height) => renderer.Resize(width, height);
			window.Render += () => renderer.Render(logic.Bounds);
			window.Update += (updatePeriod) => HandleInput(window.Input, updatePeriod, logic);
			window.Run();
		}

		private static void HandleInput(IInput input, float updatePeriod, GameLogic logic)
		{
			float axisLeftRight = input.IsButtonDown("Left") ? -1f : input.IsButtonDown("Right") ? 1f : 0f;
			float axisUpDown = input.IsButtonDown("Down") ? -1f : input.IsButtonDown("Up") ? 1f : 0f;
			logic.Update(updatePeriod, axisLeftRight, axisUpDown);
		}
	}
}