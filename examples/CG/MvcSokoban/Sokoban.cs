using OpenTK.Input;
using System;
using System.IO;
using System.Windows.Forms;
using Zenseless.Base;
using Zenseless.ExampleFramework;
using Zenseless.HLGL;

namespace MvcSokoban
{
	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			GameLogic logic;
			try
			{
				logic = (GameLogic)Serialization.FromBinFile(GetGameStateFilePath()); //try to load the last game state from a file at start of program
			}
			catch
			{
				logic = new GameLogic(window.ContentLoader.Load<string>("levels")); //loading failed -> default game state
			}
			window.GameWindow.Title = "Sokoban";
			//app.GameWindow.CursorVisible = false;
			window.GameWindow.Closing += (s, e) => Serialization.ToBinFile(logic, GetGameStateFilePath()); //save game state at end of program

			var renderer = new RendererGL4(window.RenderContext.RenderState, window.ContentLoader);
			//var renderer = new Renderer(window.RenderContext.RenderState, window.ContentManager);
			var sceneManager = new SceneManager(logic, renderer);
			window.GameWindow.KeyDown += (s, e) => sceneManager.HandleInput(KeyBindings(e.Key));
			window.Resize += (w, h) => renderer.ResizeWindow(w, h);
			window.Render += () => sceneManager.Render();
			window.Run();
			window.Dispose();
		}

		private static GameKey KeyBindings(Key key)
		{
			switch(key)
			{
				case Key.Left: return GameKey.Left;
				case Key.Right: return GameKey.Right;
				case Key.Up: return GameKey.Up;
				case Key.Down: return GameKey.Down;
				case Key.Enter: return GameKey.Accept;
				case Key.B: return GameKey.Back;
				case Key.BackSpace: return GameKey.Menu;
				case Key.R: return GameKey.Reset;
				default: return GameKey.Invalid;
			}
		}

		private static string GetGameStateFilePath()
		{
			return Path.ChangeExtension(Application.ExecutablePath, ".gameState");
		}
	}
}