using System.Drawing;

namespace Example
{
	public class SceneGame : IScene
	{
		public SceneGame(GameLogic logic, IRenderer renderer)
		{
			this.logic = logic;
			this.renderer = renderer;
		}

		public bool HandleInput(GameKey key)
		{
			switch (key)
			{
				case GameKey.Menu: return false;
				case GameKey.Reset: logic.ResetLevel(); break;
				case GameKey.Back: logic.Undo(); break;
				case GameKey.Left: logic.Update(Movement.LEFT); break;
				case GameKey.Right: logic.Update(Movement.RIGHT); break;
				case GameKey.Up: logic.Update(Movement.UP); break;
				case GameKey.Down: logic.Update(Movement.DOWN); break;
			};
			return true;
		}

		public void Render()
		{
			renderer.Clear();
			renderer.DrawLevelState(logic.GetLevelState(), Color.White);
			renderer.Print(logic.LevelNr + "/" + logic.Moves, .05f);
		}

		private readonly GameLogic logic;
		private readonly IRenderer renderer;
	}
}
