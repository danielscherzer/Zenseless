namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using OpenTK.Input;
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
			window.GameWindow.KeyDown += (s, a) => 
			{
				switch(a.Key)
				{
					case Key.Tab:
						visual.Wireframe = !visual.Wireframe;
						break;
					case Key.PageUp:
						visual.LODScale *= 1.1f;
						break;
					case Key.PageDown:
						visual.LODScale /= 1.1f;
						break;
				}
			};

			QueryObject timeQuery = new QueryObject();
			window.Render += () =>
			{
				timeQuery.Activate(QueryTarget.TimeElapsed);
				visual.Draw(camera);
				timeQuery.Deactivate();
				var timerQueryResult = timeQuery.ResultLong * 1e-6;
				window.GameWindow.Title = $"{timerQueryResult:F1}ms LODscale={visual.LODScale}";
			};
			window.Resize += visual.Resize;
			window.Run();
		}
	}
}