namespace Example
{
	using OpenTK.Input;
	using System;
	using System.Diagnostics;
	using Zenseless.ExampleFramework;
	using Zenseless.Patterns;

	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var model = new Model();
			var time = new Stopwatch();
			time.Start();

			var sampleSeries = new ExponentialSmoothing(0.01);
			var bruteForce = false;

			window.Update += (t) =>
			{
				model.UpdateMovements(t);

				var isSpaceDown = Keyboard.GetState().IsKeyDown(Key.Space);
				if(isSpaceDown != bruteForce)
				{
					sampleSeries.Clear();
					bruteForce = isSpaceDown;
				}

				var t1 = time.ElapsedTicks; //get time before collision detection
				//if (bruteForce)
				{
					model.GridCollisionCenter(); //TODO: some errors
												 //model.BruteForceCollision();
				}
				//else
				//{
				//model.GridCollision();
				//}
				var t2 = time.ElapsedTicks; //get time after collision detection
				var deltaTime = (t2 - t1) / (double)Stopwatch.Frequency;
				sampleSeries.NewSample(deltaTime);
				window.GameWindow.Title = $"{sampleSeries.SmoothedValue * 1e3:F2}ms";

				//sampleSeries.NewSample(model.CollisionCount);
				//window.GameWindow.Title = $"{sampleSeries.SmoothedValue:F0} collisions detected";
			};

			var view = new View(window.RenderContext.RenderState);
			window.Render += () => view.Render(model.Colliders);
			window.Run();

		}
	}
}