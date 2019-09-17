using OpenTK.Input;
using System;
using Zenseless.ExampleFramework;
using Zenseless.OpenGL;

namespace Reversi
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var logic = new GameLogic(); //TODO student: load the game state
			var view = new View(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.Closing += (s, e) => { /*TODO student: save the game state */ };
			window.Resize += (w, h) => view.Resize(logic.GameState, w, h);
			window.Render += () =>
			{
				var score = $"white:{logic.CountWhite} black:{logic.CountBlack}";
				window.GameWindow.Title = score;
				view.Render(logic.GameState);
				if (GameLogic.Result.PLAYING != logic.CurrentGameResult)
				{
					var winner = logic.CurrentGameResult.ToString();
					view.PrintMessage(winner);
				}
			};
			window.GameWindow.MouseDown += (s, e) =>
			{
				if (e.Button != MouseButton.Left) return; //only accept left mouse button
				var coord = window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y); //convert pixel coordinates to [-1,1]²
				var gridPos = view.CalcGridPos(new OpenTK.Vector2(coord.X, coord.Y)); //convert mouse coordinates into grid coordinates
				logic.Move(gridPos.X, gridPos.Y); //do move
			};
			window.Run();

		}
	}
}