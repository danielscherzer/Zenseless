using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.HLGL
{
	/// <summary>
	/// Interface for input handling
	/// </summary>
	public interface IInput
	{
		/// <summary>
		/// Current location of the input pointer (current mouse position by default)
		/// </summary>
		Vector2 Location { get; }

		/// <summary>
		/// Returns a list of the names of all currently pressed buttons.
		/// </summary>
		/// <value>
		/// A list of currently pressed button names.
		/// </value>
		IEnumerable<string> PressedButtons { get; }

		/// <summary>
		/// Returns <code>true</code> if the button <paramref name="name" /> is pressed.
		/// Mouse buttons are named 'Mouse Left', ...
		/// </summary>
		/// <param name="name">The name of the button.</param>
		/// <returns><code>true</code> if the button is pressed.</returns>
		bool IsButtonDown(string name);
	}
}