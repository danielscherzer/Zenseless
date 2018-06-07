namespace Example
{
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.AddWindowAspectHandling(visual.Camera.Projection);
			window.GameWindow.AddMayaCameraEvents(visual.Camera.Projection, visual.Camera.View);
			window.Render += visual.Render;
			window.Run();

		}
	}
}