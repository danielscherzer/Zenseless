using OpenTK.Input;
using System;
using Zenseless.ExampleFramework;

namespace Example
{
	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var model = new Model();
			var view = new View();
			
			window.Update += (dt) =>
			{
				// handle input
				var movementXAxis = Keyboard.GetState()[Key.Left] ? -1f : (Keyboard.GetState()[Key.Right] ? 1f : 0f);
				model.Update(movementXAxis, dt);
			};

			window.Render += () =>
			{
				view.ClearScreen();
				foreach(var shape in model.ShapeBounds) view.DrawShape(shape);
			};

			window.Resize += view.Resize;

			window.Run();
		}
	}
}