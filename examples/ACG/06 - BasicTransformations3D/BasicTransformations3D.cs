namespace Example
{
	using System;
	using Zenseless.Patterns;
	using Zenseless.ExampleFramework;

	public class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var time = new GameTime();
			window.Render += visual.Render;
			window.Update += (dt) => visual.Update(time.AbsoluteTime);
			window.Run();

		}
	}
}
