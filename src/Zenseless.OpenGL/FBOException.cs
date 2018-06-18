namespace Zenseless.OpenGL
{
	using System;

	/// <summary>
	/// Implements an FBO exception.
	/// </summary>
	/// <seealso cref="GLException" />
	[Serializable]
	public class FBOException : GLException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FBOException" /> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		public FBOException(string message) : base(message) { }
	}
}
