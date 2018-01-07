using Zenseless.Geometry;
using OpenTK;
using OpenTK.Input;
using System;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Extension method class for OpenTK.INativeWindow
	/// </summary>
	public static class INativeWindowExtensions
	{
		/// <summary>
		/// Add Maya like camera handling. 
		/// </summary>
		/// <param name="window">window that receives input system events</param>
		/// <param name="camera">orbit camera events should be routed too.</param>
		public static void AddMayaCameraEvents(this INativeWindow window, CameraOrbit camera)
		{
			window.Resize += (s, e) => camera.Aspect = (float)window.Width / window.Height;
			window.MouseMove += (s, e) =>
			{
				if (ButtonState.Pressed == e.Mouse.LeftButton)
				{
					camera.Azimuth += 300 * e.XDelta / (float)window.Width;
					camera.Elevation += 300 * e.YDelta / (float)window.Height;
				}
			};
			window.MouseWheel += (s, e) =>
			{
				if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
				{
					camera.FovY *= (float)Math.Pow(1.05, e.DeltaPrecise);
				}
				else
				{
					camera.Distance *= (float)Math.Pow(1.05, e.DeltaPrecise);
				}
			};
		}

		/// <summary>
		/// Default key bindings: ESCAPE for closing; F11 for toggling full-screen
		/// </summary>
		/// <param name="sender">window that receives input system events. Should be a <see cref="INativeWindow"/>.</param>
		/// <param name="e">The <see cref="KeyboardKeyEventArgs"/> instance containing the event data.</param>
		public static void DefaultExampleWindowKeyEvents(object sender, KeyboardKeyEventArgs e)
		{
			var window = sender as INativeWindow;
			if (window is null) return;
			switch (e.Key)
			{
				case Key.Escape:
					window.Close();
					break;
				case Key.F11:
					window.WindowState = WindowState.Fullscreen == window.WindowState ? WindowState.Normal : WindowState.Fullscreen;
					break;
			}
		}

		/// <summary>
		/// Converts pixel based coordinates to coordinates in range [-1,1]²
		/// </summary>
		/// <param name="window">window for which to convert the coordinates</param>
		/// <param name="pixelX">Window pixel x-coordinate</param>
		/// <param name="pixelY">Window pixel y-coordinate</param>
		/// <returns>Coordinates in range [-1,1]²</returns>
		public static System.Numerics.Vector2 ConvertWindowPixelCoords(this INativeWindow window, int pixelX, int pixelY)
		{
			var coord01 = new System.Numerics.Vector2(pixelX / (window.Width - 1f), 1f - pixelY / (window.Height - 1f));
			return coord01 * 2f - System.Numerics.Vector2.One;
		}
	}
}
