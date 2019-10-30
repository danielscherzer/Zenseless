using System;
using Zenseless.Patterns;
using Zenseless.ExampleFramework;
using Zenseless.HLGL;

namespace MiniGalaxyBirds
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var view = new View(window.RenderContext.RenderState, window.ContentLoader);
			GameLogic gameLogic = new GameLogic(view.Renderer);

			GameTime time = new GameTime();

			window.Update += (t) => HandleInput(window.Input, gameLogic, time.AbsoluteTime);
			window.Resize += view.ResizeWindow;
			window.Render += () => view.DrawScreen(GameLogic.visibleFrame, gameLogic.Points);
			window.Render += () => time.NewFrame();
			window.Render += () => window.GameWindow.Title = $"{time.FPS}FPS";

			window.Run();

		}

		private static void HandleInput(IInput input, GameLogic gameLogic, float time)
		{
			float axisLeftRight = input.IsButtonDown("Left") ? -1.0f : input.IsButtonDown("Right") ? 1.0f : 0.0f;
			float axisUpDown = input.IsButtonDown("Down") ? -1.0f : input.IsButtonDown("Up") ? 1.0f : 0.0f;
			bool shoot = input.IsButtonDown("Space");
			gameLogic.Update(time, axisUpDown, axisLeftRight, shoot);
		}
	}
}