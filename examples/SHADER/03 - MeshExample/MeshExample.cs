namespace Example
{
	using System;
	using Zenseless.ExampleFramework;

	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += visual.Render;
			window.Run();

		}
	}
}
