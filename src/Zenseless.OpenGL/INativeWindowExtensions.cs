namespace Zenseless.OpenGL
{
	using OpenTK;
	using OpenTK.Input;
	using System;
	using Zenseless.Geometry;

	/// <summary>
	/// Extension method class for OpenTK.INativeWindow
	/// </summary>
	public static class INativeWindowExtensions
	{
		/// <summary>
		/// Registered a perspective Transformation that will react on aspect changes of the window.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="projection">The projection.</param>
		public static void AddWindowAspectHandling(this INativeWindow window, Perspective projection)
		{
			window.Resize += (s, e) => projection.Aspect = (float)window.Width / window.Height;
		}

		/// <summary>
		/// Add Maya like camera handling for orbiting camera. 
		/// </summary>
		/// <param name="window">window that receives input system events</param>
		/// <param name="projection">Projection transformation.</param>
		/// <param name="orbit">Orbit transformation.</param>
		public static void AddMayaCameraEvents(this INativeWindow window, Perspective projection, Orbit orbit)
		{
			window.MouseMove += (s, e) =>
			{
				if (ButtonState.Pressed == e.Mouse.LeftButton)
				{
					orbit.Azimuth += 300 * e.XDelta / (float)window.Width;
					orbit.Elevation += 300 * e.YDelta / (float)window.Height;
				}
			};
			window.MouseWheel += (s, e) =>
			{
				orbit.Distance *= (float)Math.Pow(1.05, e.DeltaPrecise);
				orbit.Distance = Geometry.MathHelper.Clamp(orbit.Distance, projection.NearClip, projection.FarClip);
			};
		}

		/// <summary>
		/// Adds the first person camera events.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="camera">The camera.</param>
		public static CameraFirstPersonMovementState AddFirstPersonCameraEvents(this INativeWindow window, CameraFirstPerson camera)
		{
			window.Resize += (s, e) => camera.Aspect = (float)window.Width / window.Height;
			window.MouseMove += (s, e) =>
			{
				if (ButtonState.Pressed == e.Mouse.LeftButton)
				{
					camera.Heading += 300 * e.XDelta / (float)window.Width;
					camera.Tilt += 300 * e.YDelta / (float)window.Height;
				}
			};

			var movementState = new CameraFirstPersonMovementState();
			window.KeyDown += (s, e) =>
			{
				var delta = 1f;
				switch (e.Key)
				{
					case Key.A: movementState.X = -delta; break;
					case Key.D: movementState.X = delta; break;
					case Key.Q: movementState.Y = -delta; break;
					case Key.E: movementState.Y = delta; break;
					case Key.W: movementState.Z = -delta; break;
					case Key.S: movementState.Z = delta; break;
				}
			};
			window.KeyUp += (s, e) =>
			{
				switch (e.Key)
				{
					case Key.A: movementState.X = 0f; break;
					case Key.D: movementState.X = 0f; break;
					case Key.Q: movementState.Y = 0f; break;
					case Key.E: movementState.Y = 0f; break;
					case Key.W: movementState.Z = 0f; break;
					case Key.S: movementState.Z = 0f; break;
				}
			};
			return movementState;
		}

		/// <summary>
		/// Creates an orbiting camera controller.
		/// </summary>
		/// <param name="window">The window were the event handlers should be registered.</param>
		/// <param name="distance">The distance.</param>
		/// <param name="fieldOfViewY">The field-of-view in y-direction.</param>
		/// <param name="nearClip">The near clip plane distance.</param>
		/// <param name="farClip">The far clip plane distance.</param>
		/// <returns></returns>
		public static Camera<Orbit, Perspective> CreateOrbitingCameraController(this INativeWindow window, float distance, float fieldOfViewY = 90f, float nearClip = 0.1f, float farClip = 1f)
		{
			var perspective = new Perspective(fieldOfViewY, nearClip, farClip);
			window.AddWindowAspectHandling(perspective);
			var orbit = new Orbit(distance);
			window.AddMayaCameraEvents(perspective, orbit);

			var camera = new Camera<Orbit, Perspective>(orbit, perspective);
			return camera;
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
