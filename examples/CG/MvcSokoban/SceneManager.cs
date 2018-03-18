namespace MvcSokoban
{
	class SceneManager : IScene
	{
		private readonly SceneGame game;
		private readonly SceneMenu menu;
		private readonly GameLogic logic;
		IScene currentScene;

		public SceneManager(GameLogic logic, IRenderer renderer)
		{
			this.logic = logic;
			game = new SceneGame(logic, renderer);
			menu = new SceneMenu(logic, renderer);
			currentScene = game;
		}

		public bool HandleInput(GameKey key)
		{
			if (!currentScene.HandleInput(key))
			{
				if (currentScene == game)
				{
					currentScene = menu;
					logic.ResetLevel();
				}
				else if (currentScene == menu)
				{
					currentScene = game;
				}
			}
			return true;
		}

		public void Render()
		{
			currentScene.Render();
		}
	}
}
