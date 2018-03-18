namespace MvcSokoban
{
	public enum GameKey { Up, Down, Left, Right, Accept, Back, Reset, Menu, Invalid };

	public interface IScene
	{
		bool HandleInput(GameKey key);
		void Render();
	}
}