using Zenseless.ExampleFramework;
using Zenseless.Patterns;
using System;
using System.IO;
using System.Windows.Forms;
using Zenseless.OpenGL;

namespace Example
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			GameState gameState;
			try
			{
				gameState = (GameState)Serialization.FromBinFile(GetGameStateFilePath()); //try to load the game state from a file at start of program
			}
			catch
			{
				gameState = new GameState(); //loading failed -> reset
			}

			window.GameWindow.Closing += (s, e) => Serialization.ToBinFile(gameState, GetGameStateFilePath()); //save game state at end of program
			window.GameWindow.KeyDown += (s, e) => { if (e.Key == OpenTK.Input.Key.R) gameState = new GameState(); }; //reset
			window.GameWindow.MouseDown += (s, e) => 
			{
				var coord = window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y); //convert pixel coordinates to [-1,1]²
				HandleInput(gameState, (int)e.Button, coord.X * .5f + .5f, coord.Y * .5f + .5f);
			};
			//todo student: app.Resize += (width, height) => //todo student: react on window changes (update apsect ratio of game)
			window.Render += () => Visual.DrawScreen(gameState); //this draws the game using OpenGL
			//app.Render += () => VisualConsole.DrawScreen(gameState); //this draws the game to the console
			window.Run();

		}

		private static void HandleInput(GameState gameState, int button, float x, float y)
		{
			//transform normalized coordinates to grid coordinates
			var gridX = (int)(x * gameState.GridWidth);
			var gridY = (int)(y * gameState.GridHeight);
			FieldType field;
			switch (button)
			{
				case 0:
					field = FieldType.CROSS;
					break;
				case 1:
					field = FieldType.DIAMONT;
					break;
				default:
					field = FieldType.EMPTY;
					break;
			}
			gameState[gridX, gridY] = field;
		}

		private static string GetGameStateFilePath()
		{
			return Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "gameState.bin";
		}
	}
}