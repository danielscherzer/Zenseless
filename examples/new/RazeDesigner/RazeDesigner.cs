namespace Example
{
	using OpenTK.Input;
	using System;
	using System.Numerics;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var model = new Model();
			var visual = new Visual(window.RenderContext.RenderState, window.ContentLoader);

			Vector2 ConvertPixelToModelCoords(int x, int y)
			{
				var coordWindow = window.GameWindow.ConvertWindowPixelCoords(x, y);
				return visual.ConvertWindowCoords(coordWindow);
			}

			window.GameWindow.MouseDown += (s, e) =>
			{
				var coord = ConvertPixelToModelCoords(e.X, e.Y);
				switch(e.Button)
				{
					case MouseButton.Left:
						model.BeginEdit(coord);
						break;
					case MouseButton.Right:
						model.Delete(coord);
						break;
				}
			};
			window.GameWindow.MouseUp += (s, e) =>
			{
				if (MouseButton.Left == e.Button)
				{
					model.EndEdit();
				}
			};
			window.GameWindow.MouseMove += (s, e) =>
			{
				var coord = ConvertPixelToModelCoords(e.X, e.Y);
				model.Move(coord);
			};
			float truckPosition = 0f;
			window.Update += (dt) =>
			{
				truckPosition += dt;
				truckPosition = truckPosition % (model.Points.Count - 1);
			};
			window.Render += () => visual.Render(model.Points, model.SelectedPoint, truckPosition);
			window.Resize += visual.Resize;
			//window.GameWindow.WindowState = OpenTK.WindowState.Fullscreen;
			window.Run();
			window.Dispose();
		}
	}
}