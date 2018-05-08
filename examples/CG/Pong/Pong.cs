using OpenTK.Input;
using System;
using Zenseless.ExampleFramework;

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
			window.Update += (t) => Window_Update(logic, t);
			window.Render += () => visual.Render(logic.Paddle1, logic.Paddle2, logic.Ball, logic.Player1Points, logic.Player2Points);
			window.Run();

		}

		private static void Window_Update(Logic logic, float updatePeriod)
		{
			var reset = Keyboard.GetState()[Key.Space];
			float axisPaddle1 = Keyboard.GetState()[Key.A] ? -1.0f : Keyboard.GetState()[Key.Q] ? 1.0f : 0.0f;
			float axisPaddle2 = Keyboard.GetState()[Key.L] ? -1.0f : Keyboard.GetState()[Key.O] ? 1.0f : 0.0f;
			logic.Update(updatePeriod, reset, axisPaddle1, axisPaddle2);
		}
	}
}