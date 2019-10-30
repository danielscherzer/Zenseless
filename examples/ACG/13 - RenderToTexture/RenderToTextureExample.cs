namespace Example
{
	using OpenTK.Input;
	using System;
	using Zenseless.Patterns;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	class Controller
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();

			var camera = window.GameWindow.CreateOrbitingCameraController(1.8f, 70, 0.1f, 50f);
			camera.View.TargetY = -0.3f;
			var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

			var globalTime = new GameTime();
			bool doPostProcessing = false;

			window.Render += () =>
			{
				if (doPostProcessing)
				{
					visual.DrawWithPostProcessing(globalTime.AbsoluteTime, camera);
				}
				else
				{
					visual.Draw(camera);
				}
			};

			window.Update += (t) => doPostProcessing = !window.Input.IsButtonDown("Space");
			window.Resize += visual.Resize;
			window.Run();

		}
	}
}