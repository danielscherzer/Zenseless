namespace Example
{
	using System;
	using Zenseless.ExampleFramework;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MainVisual(window.ContentLoader);
			window.Render += visual.Render;
			window.Run();
		}
	}
}