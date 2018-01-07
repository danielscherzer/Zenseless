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
		/// Creates the draw configuration.
		/// </summary>
		/// <returns></returns>
		IDrawConfiguration CreateDrawConfiguration();
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
		/// Creates the shader.
		/// </summary>
		/// <returns></returns>
		IShader CreateShader();
		/// <summary>
		/// Draws the points.
		/// </summary>
		/// <param name="count">The count.</param>
		void DrawPoints(int count);
		/// <summary>
		/// Gets the frame buffer.
		/// </summary>
		/// <returns></returns>
		IRenderSurface GetFrameBuffer();
	}
}