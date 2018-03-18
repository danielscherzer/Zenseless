namespace Example
{
	using OpenTK.Input;
	using System;
	using Zenseless.ExampleFramework;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var model = new Model();
			var visual = new Visual(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.KeyDown += (s, a) =>
			{
				if(a.Key == Key.Space)
				{
					model.CreatePath();
				}
			};
			window.Render += () => visual.Render(model.Paths, model.Points);
			window.GameWindow.WindowState = OpenTK.WindowState.Fullscreen;
			window.Run();
		}
	}
}