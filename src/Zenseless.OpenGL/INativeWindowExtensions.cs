namespace Zenseless.OpenGL
{
	using OpenTK;
	using OpenTK.Input;
	using OpenTK.Platform;
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
		/// 
		/// </summary>
		public class FirstPersonMovementState
		{
			/// <summary>
			/// delta movement since
			/// </summary>
			public System.Numerics.Vector3 movement;
		};
		/// <summary>
		/// Add first person camera controller. 
		/// </summary>
		/// <param name="window">window that receives input system events</param>
		/// <param name="firstPerson">The first person transform.</param>
		public static FirstPersonMovementState AddFirstPersonCameraEvents(this INativeWindow window, FirstPerson firstPerson)
		{
			window.MouseMove += (s, e) =>
			{
				if (ButtonState.Pressed == e.Mouse.LeftButton)
				{
					firstPerson.Heading += 300 * e.XDelta / (float)window.Width;
					firstPerson.Tilt += 300 * e.YDelta / (float)window.Height;
				}
			};
			var state = new FirstPersonMovementState();
			window.KeyDown += (s, e) =>
			{
				var movement = state.movement;
				var delta = 1f;
				switch (e.Key)
				{
					case Key.A: movement.X = -delta; break;
					case Key.D: movement.X = delta; break;
					case Key.Q: movement.Y = -delta; break;
					case Key.E: movement.Y = delta; break;
					case Key.W: movement.Z = -delta; break;
					case Key.S: movement.Z = delta; break;
				}
				state.movement = movement;
			};
			window.KeyUp += (s, e) =>
			{
				var movement = state.movement;
				switch (e.Key)
				{
					case Key.A: movement.X = 0f; break;
					case Key.D: movement.X = 0f; break;
					case Key.Q: movement.Y = 0f; break;
					case Key.E: movement.Y = 0f; break;
					case Key.W: movement.Z = 0f; break;
					case Key.S: movement.Z = 0f; break;
				}
				state.movement = movement;
			};
			return state;
		}

		/// <summary>
		/// Creates a first person camera controller.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="speed">The movement speed.</param>
		/// <param name="position">The starting position.</param>
		/// <param name="fieldOfViewY">The field of view y.</param>
		/// <param name="nearClip">The near clip plane.</param>
		/// <param name="farClip">The far clip plane.</param>
		/// <returns>a Camera</returns>
		public static Camera<FirstPerson, Perspective> CreateFirstPersonCameraController(this IGameWindow window, float speed, System.Numerics.Vector3 position, float fieldOfViewY = 90f, float nearClip = 0.1f, float farClip = 1f)
		{
			var perspective = new Perspective(fieldOfViewY, nearClip, farClip);
			var camera = new Camera<FirstPerson, Perspective>(new FirstPerson(position), perspective);

			window.AddWindowAspectHandling(perspective);
			var movementState = window.AddFirstPersonCameraEvents(camera.View);
			window.UpdateFrame += (s, a) => camera.View.ApplyRotatedMovement(movementState.movement * speed * (float)a.Time);

			return camera;
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
			var orbit = new Orbit(distance);
			var camera = new Camera<Orbit, Perspective>(orbit, perspective);

			window.AddWindowAspectHandling(perspective);
			window.AddMayaCameraEvents(perspective, orbit);

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
