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
			var window = new ExampleWindow(debug:true);
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.GameWindow.AddWindowAspectHandling(visual.Camera.Projection);
			var movementState = window.GameWindow.AddFirstPersonCameraEvents(visual.Camera.View);

			window.Update += (dt) => visual.Camera.View.ApplyRotatedMovement(movementState.movement * 30 * dt);
			var sampleSeries = new ExponentialSmoothing(0.01);
			QueryObject timeQuery = new QueryObject();
			window.Render += () =>
			{
				timeQuery.Activate(QueryTarget.TimeElapsed);
				visual.Draw();
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