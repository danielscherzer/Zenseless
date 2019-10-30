using System;
using Zenseless.ExampleFramework;
using Zenseless.HLGL;

namespace Pong
{
	class Pong
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var logic = new Logic();
			var visual = new MyVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Update += (t) => Window_Update(window.Input, logic, t);
			window.Render += () => visual.Render(logic.Paddle1, logic.Paddle2, logic.Ball, logic.Player1Points, logic.Player2Points);
			window.Run();

		}

		private static void Window_Update(IInput input, Logic logic, float updatePeriod)
		{
			var reset = input.IsButtonDown("Space");
			float axisPaddle1 = input.IsButtonDown("A") ? -1f : input.IsButtonDown("Q") ? 1f : 0f;
			float axisPaddle2 = input.IsButtonDown("L") ? -1f : input.IsButtonDown("O") ? 1f : 0f;
			logic.Update(updatePeriod, reset, axisPaddle1, axisPaddle2);
		}
	}
}