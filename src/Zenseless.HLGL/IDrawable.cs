namespace Zenseless.HLGL
{
	/// <summary>
	/// An interface that allows instanced drawing
	/// </summary>
	public interface IDrawable
	{
		/// <summary>
		/// Draws the <seealso cref="IDrawable"/> instance count times.
		/// </summary>
		/// <param name="instanceCount">The instance count (default is 1).</param>
		void Draw(int instanceCount);

		/// <summary>
		/// Draws the <seealso cref="IDrawable"/>. If instance attributes are present it will be drawn instance count times.
		/// </summary>
		void Draw();
	}
}
