namespace Zenseless.OpenGL
{
	using System;

	/// <summary>
	/// Implements a base GL exception.
	/// </summary>
	/// <seealso cref="Exception" />
	[Serializable]
	public class GLException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GLException" /> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		public GLException(string message) : base(message) { }
	}
}
