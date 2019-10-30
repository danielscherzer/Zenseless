namespace Example
{
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.HLGL;

	class TiledLevelExample
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			var dir = ContentSearchDirectoryAttribute.GetContentSearchDirectory();
			var tileMap = new TileMap(dir + @"\Content\level.tmx");
			var model = new Model(tileMap.ExtractStart(), tileMap.ExtractCollisionGrid());
			var view = new View(window.ContentLoader, window.RenderContext.RenderState, tileMap.ExtractTileSprites(), tileMap.ExtractViewLayerGrids());

			window.GameWindow.MouseWheel += (s, e) => view.Zoom *= (float)Math.Pow(1.05, 3 * e.DeltaPrecise);

			float MovementAxis(float dt, string keyMinus, string keyPlus)
			{
				return window.Input.IsButtonDown(keyPlus) ? dt : window.Input.IsButtonDown(keyMinus) ? -dt : 0f;
			}
			window.Update += (dt) =>
			{
				Console.WriteLine(string.Join("#", window.Input.PressedButtons));
				float deltaX = MovementAxis(dt, "Left", "Right");
				float deltaY = MovementAxis(dt, "Down", "Up");
				model.Update(deltaX, deltaY);
			};
			window.Render += () =>
			{
				view.Draw(model.Player);
			};
			window.Resize += view.Resize;
			window.Run();
		}
	}
}
