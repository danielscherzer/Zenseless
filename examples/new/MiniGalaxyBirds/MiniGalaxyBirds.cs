using OpenTK.Input;
using System;
using Zenseless.Base;
using Zenseless.ExampleFramework;

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

			window.Update += (t) => HandleInput(gameLogic, time.AbsoluteTime);
			window.Resize += view.ResizeWindow;
			window.Render += () => view.DrawScreen(GameLogic.visibleFrame, gameLogic.Points);
			window.Render += () => time.NewFrame();
			window.Render += () => window.GameWindow.Title = $"{time.FPS}FPS";

			window.Run();
		}

		private static void HandleInput(GameLogic gameLogic, float time)
		{
			float axisUpDown = Keyboard.GetState()[Key.Up] ? -1.0f : Keyboard.GetState()[Key.Down] ? 1.0f : 0.0f;
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];
			gameLogic.Update(time, axisUpDown, axisLeftRight, shoot);
		}
	}
}