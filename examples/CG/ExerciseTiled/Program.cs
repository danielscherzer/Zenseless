namespace Example
{
	using OpenTK.Input;
	using Zenseless.ExampleFramework;

	class Program
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			var tileMap = new TileMap();
			var view = new View(window.ContentLoader, window.RenderContext.RenderState, tileMap.SpriteSheetName);
			window.Render += () => view.Draw(tileMap.Tiles);
			window.Run();
		}
	}
}
