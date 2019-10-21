namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Numerics;
	using Zenseless.ExampleFramework;
	using Zenseless.Geometry;
	using Zenseless.OpenGL;
	using Zenseless.Patterns;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow(debug:true);

			var camera = window.GameWindow.CreateFirstPersonCameraController(1f, new Vector3(36f, 0.1f, 30f), 70f, 0.01f, 300f);

			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.KeyDown += (s, a) => { if (a.Key == OpenTK.Input.Key.Tab) visual.Wireframe = !visual.Wireframe; };

			var sampleSeries = new ExponentialSmoothing(0.01);
			QueryObject timeQuery = new QueryObject();
			window.Render += () =>
			{
				timeQuery.Activate(QueryTarget.TimeElapsed);
				visual.Draw(camera);
				timeQuery.Deactivate();
				var timerQueryResult = timeQuery.ResultLong * 1e-6;
				sampleSeries.NewSample(timerQueryResult);
				window.GameWindow.Title = $"{sampleSeries.SmoothedValue:F0}ms";
			};
			window.Resize += visual.Resize;
			window.Resize += (w, h) => sampleSeries.Clear();
			window.Run();
		}
	}
}