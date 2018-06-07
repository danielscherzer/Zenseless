namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;
	using Zenseless.Patterns;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();

			var camera = window.GameWindow.CreateOrbitingCameraController(0.3f, 70, 0.01f, 30f);
			camera.View.Elevation = 15;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var sampleSeries = new ExponentialSmoothing(0.01);
			QueryObject timeQuery = new QueryObject();
			window.Render += () =>
			{
				var timerQueryResult = timeQuery.ResultLong * 1e-6;
				sampleSeries.NewSample(timerQueryResult);
				window.GameWindow.Title = $"{sampleSeries.SmoothedValue:F0}ms";
				timeQuery.Activate(QueryTarget.TimeElapsed);
				visual.Draw(camera);
				timeQuery.Deactivate();
			};
			window.Resize += visual.Resize;
			window.Resize += (w, h) => sampleSeries.Clear();
			window.Run();
		}
	}
}