using Zenseless.ExampleFramework;
using System;

namespace Example
{
	class Controler
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new GameVisual(window.RenderContext.RenderState, window.ContentLoader);
			var postProcessing = new PostProcessingVisual(window.GameWindow.Width, window.GameWindow.Height, window.ContentLoader);
			window.Update += visual.Update;
			window.Render += () =>
			{
				bool doPostProcessing = !window.Input.IsButtonDown("Space");
				if (doPostProcessing)
				{
					postProcessing.Render(visual.Render);
				}
				else
				{
					visual.Render();
				}
			};
			window.Run();

		}
	}
}