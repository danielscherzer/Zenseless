using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;

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

			StateManager = new StateManager();
			StateManager.Register<StateActiveFboGL, StateActiveFboGL>(new StateActiveFboGL());
			StateManager.Register<StateActiveShaderGL, StateActiveShaderGL>(new StateActiveShaderGL());
			StateManager.Register<IStateTyped<float>, States.ILineWidth>(new StateCommandGL<float>(GL.LineWidth, 1f));
		}

		/// <summary>
		/// Gets the state manager.
		/// </summary>
		/// <value>
		/// The state manager.
		/// </value>
		public IStateManager StateManager { get; private set; }

		/// <summary>
		/// Gets the state of the render.
		/// </summary>
		/// <value>
		/// The state of the render.
		/// </value>
		public IRenderState RenderState { get; private set; }

		/// <summary>
		/// Draws the points.
		/// </summary>
		/// <param name="count">The count.</param>
		public void DrawPoints(int count)
		{
			GL.DrawArrays(PrimitiveType.Points, 0, count);
		}

		/// <summary>
		/// Gets the frame buffer.
		/// </summary>
		/// <returns></returns>
		public IRenderSurface GetFrameBuffer()
		{
			return new RenderSurfaceGL();
		}

		/// <summary>
		/// Creates the draw configuration.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public IDrawConfiguration CreateDrawConfiguration()
		{
			throw new NotImplementedException();
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
		public IRenderSurface CreateRenderSurface(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false)
		{
			return new RenderSurfaceGL(width, height, hasDepthBuffer, components, floatingPoint);
		}

		/// <summary>
		/// Creates the shader.
		/// </summary>
		/// <returns></returns>
		public IShader CreateShader()
		{
			return new Shader();
		}
	}
}
