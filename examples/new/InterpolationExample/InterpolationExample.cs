using System;
using Zenseless.ExampleFramework;
using Zenseless.Base;

namespace Example
{
	class Program
	{ 
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var model = new Model();
			var visual = new MyVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => visual.Render(model.MovingObject);
			window.Update += (dt) => model.Update(time.AbsoluteTime);
			window.Run();

		}
	}
}