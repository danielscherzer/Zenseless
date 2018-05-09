namespace Example
{
	using System;
	using Zenseless.Base;
	using Zenseless.ExampleFramework;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => visual.Render(time.AbsoluteTime);
			window.Resize += visual.Resize;
			window.Run();

		}
	}
}