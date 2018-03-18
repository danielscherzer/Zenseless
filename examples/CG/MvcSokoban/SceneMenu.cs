using System.Drawing;

namespace MvcSokoban
{
	public class SceneMenu : IScene
	{
		public SceneMenu(GameLogic logic, IRenderer renderer)
		{
			this.logic = logic;
			this.renderer = renderer;
		}

		public bool HandleInput(GameKey key)
		{
			switch (key)
			{
				case GameKey.Right: ++logic.LevelNr; break;
				case GameKey.Left: --logic.LevelNr; break;
				case GameKey.Accept: return false;
			};
			return true;
		}

		public void Render()
		{
			renderer.Clear();
			renderer.DrawLevelState(logic.GetLevelState(), Color.Gray);
			if(logic.HasLevelBeenWon(logic.LevelNr))
			{
				renderer.Print("Level " + logic.LevelNr + " (WON)", .1f, TextAlignment.Center);
			}
			else
			{
				renderer.Print("Level " + logic.LevelNr, .1f, TextAlignment.Center);
			}
		}

		private readonly GameLogic logic;
		private readonly IRenderer renderer;
	}
}
