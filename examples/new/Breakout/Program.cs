using OpenTK.Input;
using Zenseless.ExampleFramework;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			var logic = new Logic();
			var view = new View();
			window.Update += (updatePeriod) =>
			{
				float axisPaddle = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
				if (!logic.Update(updatePeriod, axisPaddle)) window.GameWindow.Close();
			};
			window.Render += () =>
			{
				view.Clear();
				foreach (var brick in logic.Bricks) view.DrawBox(brick);
				view.DrawBox(logic.Paddle);
				view.DrawBall(logic.Ball);
			};
			window.Run();
		}
	}
}
