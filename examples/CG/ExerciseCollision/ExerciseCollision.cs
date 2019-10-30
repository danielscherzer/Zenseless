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
				float axisLeftRight = window.Input.IsButtonDown("Left") ? -1f : window.Input.IsButtonDown("Right") ? 1f : 0f;
				model.Update(axisLeftRight, dt);
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