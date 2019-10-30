using OpenTK.Input;
using System.Collections.Generic;
using System.Numerics;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Class for input logic. Mouse and keyboard buttons can be queried.
	/// </summary>
	public class Input : IInput
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Input"/> class.
		/// </summary>
		/// <param name="window">The window.</param>
		public Input(OpenTK.INativeWindow window)
		{
			this.window = window;
			window.KeyDown += (s, a) => pressedButtons.Add(a.Key.ToString());
			window.KeyUp += (s, a) => pressedButtons.Remove(a.Key.ToString());
			window.MouseDown += Window_MouseDown;
			window.MouseUp += Window_MouseUp;
			window.MouseMove += Window_MouseMove;
		}

		/// <summary>
		/// Current location of the input pointer (current mouse position by default)
		/// </summary>
		public Vector2 Location { get; private set; }

		/// <summary>
		/// Returns a list of the names of all currently pressed buttons.
		/// </summary>
		/// <value>
		/// A list of currently pressed button names.
		/// </value>
		public IEnumerable<string> PressedButtons => pressedButtons;

		/// <summary>
		/// Converts pixel based coordinates to coordinates in range [-1,1]²
		/// </summary>
		/// <param name="point">Window pixel coordinate</param>
		/// <returns>Coordinates in range [-1,1]²</returns>
		public Vector2 ConvertWindowPixelCoords(OpenTK.Point point)
		{
			var coord01 = new Vector2(point.X / (window.Width - 1f), 1f - point.Y / (window.Height - 1f));
			return coord01 * 2f - Vector2.One;
		}

		/// <summary>
		/// Returns <code>true</code> if the button <paramref name="name" /> is pressed.
		/// Mouse buttons are named 'Mouse Left', ...
		/// </summary>
		/// <param name="name">The name of the button.</param>
		/// <returns><code>true</code> if the button is pressed.</returns>
		public bool IsButtonDown(string name)
		{
			return pressedButtons.Contains(name);
		}

		private HashSet<string> pressedButtons = new HashSet<string>();
		private readonly OpenTK.INativeWindow window;

		private string ConvertToName(MouseButton button) => $"Mouse {button}";

		private void Window_MouseMove(object sender, MouseMoveEventArgs e)
		{
			Location = ConvertWindowPixelCoords(e.Position);
		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			pressedButtons.Remove(ConvertToName(e.Button));
			Location = ConvertWindowPixelCoords(e.Position);
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			pressedButtons.Add(ConvertToName(e.Button));
			Location = ConvertWindowPixelCoords(e.Position);
		}
	}
}
