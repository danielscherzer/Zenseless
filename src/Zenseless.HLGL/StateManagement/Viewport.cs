namespace Zenseless.HLGL
{
	/// <summary>
	/// State of the view port rectangle.
	/// </summary>
	public struct Viewport
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Viewport"/> structure.
		/// </summary>
		/// <param name="x">Specify the left corner of the view port rectangle, in pixels. The initial value is 0.</param>
		/// <param name="y">Specify the lower corner of the view port rectangle, in pixels. The initial value is 0.</param>
		/// <param name="width">Specify the width of the view port. When a GL context is first attached to a window, width and height are set to the dimensions of that window.</param>
		/// <param name="height">Specify the height of the view port. When a GL context is first attached to a window, width and height are set to the dimensions of that window.</param>
		public Viewport(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Specify the left corner of the view port rectangle, in pixels. The initial value is 0.
		/// </summary>
		/// <value>
		/// The x.
		/// </value>
		public int X { get; }

		/// <summary>
		/// Specify the lower corner of the view port rectangle, in pixels. The initial value is 0.
		/// </summary>
		/// <value>
		/// The y.
		/// </value>
		public int Y { get; }

		/// <summary>
		/// The width of the view port.
		/// </summary>
		/// <value>
		/// The width of the view port.
		/// </value>
		public int Width { get; }

		/// <summary>
		/// The height of the view port. 
		/// </summary>
		/// <value>
		/// The height of the view port.
		/// </value>
		public int Height { get; }
	}
}
