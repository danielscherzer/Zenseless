namespace Example
{
	using OpenTK.Input;
	using System;
	using System.IO;
	using Zenseless.Base;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath()));
			var model = new Model();
			var visual = new Visual(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.MouseDown += (s, e) =>
			{
				var coord = window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y);
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
				var coord = window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y);
				model.Move(coord);
			};
			window.Render += () => visual.Render(model.Points, model.SelectedPoint);
			window.Resize += visual.Resize;
			window.GameWindow.WindowState = OpenTK.WindowState.Fullscreen;
			window.Run();
		}
	}
}