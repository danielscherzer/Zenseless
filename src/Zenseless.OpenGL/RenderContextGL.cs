using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IRenderContext" />
	public class RenderContextGL : IRenderContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RenderContextGL"/> class.
		/// </summary>
		public RenderContextGL()
		{
			RenderState = RenderStateGL.Create();
		}

		/// <summary>
		/// Gets the state of the render.
		/// </summary>
		/// <value>
		/// The state of the render.
		/// </value>
		public IRenderState RenderState { get; private set; }

		/// <summary>
		/// Gets the frame buffer.
		/// </summary>
		/// <returns></returns>
		public IOldRenderSurface GetFrameBuffer()
		{
			return new RenderSurfaceGL();
		}

		/// <summary>
		/// Creates the render surface.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="hasDepthBuffer">if set to <c>true</c> [has depth buffer].</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		/// <returns></returns>
		public IOldRenderSurface CreateRenderSurface(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false)
		{
			return new RenderSurfaceGL(width, height, hasDepthBuffer, components, floatingPoint);
		}
	}
}
