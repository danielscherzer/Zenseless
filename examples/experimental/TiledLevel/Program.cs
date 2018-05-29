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
			var model = new Model(tileMap.Tiles);
			var view = new View(window.ContentLoader, window.RenderContext.RenderState, tileMap.TileTypes, tileMap.SpriteSheetName);
			window.Update += (dt) =>
			{
				float deltaZoom = MovementAxis(dt, Key.A, Key.Q);
				view.Zoom += deltaZoom;
				float deltaX = MovementAxis(dt, Key.Left, Key.Right);
				float deltaY = MovementAxis(dt, Key.Down, Key.Up);
				model.Update(deltaX, deltaY);
			};
			window.Render += () =>
			{
				view.Draw(tileMap.Tiles, model.Player);
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
