namespace Example
{
	using OpenTK.Input;
	using System;
	using System.Numerics;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			//window.GameWindow.Bounds = new System.Drawing.Rectangle(2000, 1000, 1000, 1000); //TODO: for debug
			var model = new Model();
			var view = new View(window.RenderContext, window.ContentLoader);

			var selectionStart = Vector2.Zero;
			var selectionEnd = Vector2.Zero;
			window.GameWindow.MouseDown += (s, e) =>
			{
				var coord = view.TransformToModel(window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y));
				switch (e.Button)
				{
					case MouseButton.Left:
						selectionStart = coord;
						selectionEnd = coord;
						break;
					case MouseButton.Right:
						model.MoveTo(coord);
						break;
				}
			};
			window.GameWindow.MouseUp += (s, e) =>
			{
				if (MouseButton.Left == e.Button)
				{
					model.Select(selectionStart, selectionEnd);
					selectionStart = Vector2.Zero;
					selectionEnd = Vector2.Zero;
				}
			};
			window.GameWindow.MouseMove += (s, e) =>
			{
				if(e.Mouse.IsButtonDown(MouseButton.Left))
				{
					var coord = view.TransformToModel(window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y));
					selectionEnd = coord;
					model.Select(selectionStart, selectionEnd);
				}
			};
			window.Render += () =>
			{
				view.Clear();
				foreach(var unit in model.Units)
				{
					view.DrawUnit(unit);
				}
				view.DrawRect(selectionStart, selectionEnd);
			};
			window.Update += model.Update;
			window.Run();

		}
	}
}