using System;
using Zenseless.ExampleFramework;

namespace Example
{
	class Program
	{ 
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var logic = new Model();
			var visual = new MyVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Update += logic.Update;
			window.Render += () => visual.Render(logic.GetPlanets());
			window.Run();

		}
	}
}