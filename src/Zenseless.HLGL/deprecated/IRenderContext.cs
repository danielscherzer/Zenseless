namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IRenderContext
	{
		/// <summary>
		/// Gets the state manager.
		/// </summary>
		/// <value>
		/// The state manager.
		/// </value>
		IStateManager StateManager { get; }

		/// <summary>
		/// Gets the state of the render.
		/// </summary>
		/// <value>
		/// The state of the render.
		/// </value>
		IRenderState RenderState { get; }

		/// <summary>
		/// Creates the render surface.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="hasDepthBuffer">if set to <c>true</c> [has depth buffer].</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		/// <returns></returns>
		IRenderSurface CreateRenderSurface(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false);

		/// <summary>
		/// Draws the points.
		/// </summary>
		/// <param name="count">The count.</param>
		void DrawPoints(int count);

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		TYPE Get<TYPE>(string name) where TYPE : class;

		/// <summary>
		/// Gets the frame buffer.
		/// </summary>
		/// <returns></returns>
		IRenderSurface GetFrameBuffer();
	}
}