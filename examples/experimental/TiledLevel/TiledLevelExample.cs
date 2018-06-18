namespace Example
{
	using OpenTK.Input;
	using System;
	using Zenseless.ExampleFramework;

	class TiledLevelExample
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			var tileMap = new TileMap();
			var model = new Model(tileMap.ExtractStart(), tileMap.ExtractCollisionGrid());
			var view = new View(window.ContentLoader, window.RenderContext.RenderState, tileMap.ExtractTileSprites(), tileMap.ExtractViewLayerGrids());

			window.GameWindow.MouseWheel += (s, e) => view.Zoom *= (float)Math.Pow(1.05, 3 * e.DeltaPrecise);

			window.Update += (dt) =>
			{
				float deltaX = MovementAxis(dt, Key.Left, Key.Right);
				float deltaY = MovementAxis(dt, Key.Down, Key.Up);
				model.Update(deltaX, deltaY);
			};
			window.Render += () =>
			{
				view.Draw(model.Player);
			};
			window.Resize += view.Resize;
			window.Run();
		}

		private static float MovementAxis(float dt, Key keyMinus, Key keyPlus)
		{
			return Keyboard.GetState().IsKeyDown(keyPlus) ? dt : Keyboard.GetState().IsKeyDown(keyMinus) ? -dt : 0f;
		}
	}
}
