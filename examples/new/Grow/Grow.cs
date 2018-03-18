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
			var model = new Model(1000);
			var renderer = new Renderer(window.RenderContext.RenderState, window.ContentLoader);

			window.Update += (time) => model.Update(time);
			window.Resize += renderer.Resize;
			window.Render += () =>
			{
				renderer.Clear();
				renderer.DrawElements(model.Elements);
				renderer.DrawPlayer(model.Player);
			};
			window.Run();
		}
	}
}