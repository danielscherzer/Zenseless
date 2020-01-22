using OpenTK.Input;
using System;
using System.Linq;
using System.Numerics;
using Zenseless.ExampleFramework;
using Zenseless.Geometry;
using Zenseless.OpenGL;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new ExampleWindow();
			var view = new View(window.ContentLoader, window.RenderContext);
			var bounds = view.Bounds;
			var distance = 1.5f * Math.Max(bounds.SizeX, Math.Max(bounds.SizeY, bounds.SizeZ));
			var camera = window.GameWindow.CreateOrbitingCameraController(distance, 70f, 1f, 2000f);
			camera.View.Target = new Vector3(bounds.CenterX, bounds.CenterY, bounds.CenterZ);
			var useViewCamera = false;
			Action draw = () => view.Draw(camera, camera.View.CalcPosition());
			window.GameWindow.KeyDown += (s, a) =>
			{
				if(Key.C == a.Key)
				{
					useViewCamera = !useViewCamera;
					if (useViewCamera && 0 < view.Cameras.Count())
					{
						draw = () =>
						{
							var cam = view.Cameras.First();
							var pos = cam.Matrix.Translation;
							view.Draw(cam, pos);
						};
					}
					else
					{
						draw = () => view.Draw(camera, camera.View.CalcPosition());
					}
				}
			};
			window.Render += () => draw();
			window.Run();

		}
	}
}
