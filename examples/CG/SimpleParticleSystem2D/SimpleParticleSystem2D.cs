using OpenTK.Input;
using System;
using System.Drawing;
using Zenseless.ExampleFramework;
using Zenseless.OpenGL;

namespace Example
{
	class Program
	{
		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var model = new Model(1000);
			var renderer = new Renderer();

			void updateSourceLocation(MouseEventArgs e)
			{
				model.Emitter = window.GameWindow.ConvertWindowPixelCoords(e.X, e.Y); //convert pixel coordinates to [-1,1]²
			}

			//move the mouse to move the particle source(emitter)
			window.GameWindow.MouseMove += (s, a) => updateSourceLocation(a);
			window.GameWindow.MouseDown += (s, a) => updateSourceLocation(a);

			window.Update += (time) => model.Update(time);
			window.Resize += renderer.Resize;
			window.Render += () =>
			{
				renderer.Clear();
				foreach (var particle in model.Particles)
				{
					renderer.DrawPoint(particle.Location, particle.Age);
				}
				renderer.DrawPoint(model.Emitter, Color.Red);
			};
			window.Run();
		}
	}
}