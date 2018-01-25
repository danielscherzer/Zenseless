using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using System;

namespace Zenseless.OpenGL
{
	//public interface IClearColor : ICommand { };

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.HLGL.IRenderContext" />
	public class RenderContextGL : IRenderContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RenderContextGL"/> class.
		/// </summary>
		public RenderContextGL()
		{
			StateManager = new StateManager();
			StateManager.Register<StateActiveFboGL, StateActiveFboGL>(new StateActiveFboGL());
			StateManager.Register<StateActiveShaderGL, StateActiveShaderGL>(new StateActiveShaderGL());
			StateManager.Register<IStateBool, States.IZBufferTest>(new StateBoolGL(EnableCap.DepthTest));
			StateManager.Register<IStateBool, States.IBackfaceCulling>(new StateBoolGL(EnableCap.CullFace));
			StateManager.Register<IStateBool, States.IShaderPointSize>(new StateBoolGL(EnableCap.ProgramPointSize));
			StateManager.Register<IStateBool, States.IPointSprite>(new StateBoolGL(EnableCap.PointSprite));
			StateManager.Register<IStateBool, States.IBlending>(new StateBoolGL(EnableCap.Blend)); //TODO: use blend state
			StateManager.Register<IStateTyped<float>, States.ILineWidth>(new StateCommandGL<float>(GL.LineWidth, 1f));
			StateManager.Register<IStateTyped<Vector4>, States.IClearColor>(new StateCommandGL<Vector4>(ClearColor, Vector4.Zero));
			//stateManager.Register<ICommand, IClearColor>(new CommandGL());
			//StateManager.Register<ICreator<IShader>, IShader>(new ShaderCreatorGL());
		}

		/// <summary>
		/// Gets the state manager.
		/// </summary>
		/// <value>
		/// The state manager.
		/// </value>
		public IStateManager StateManager { get; private set; }

		/// <summary>
		/// Clears the color.
		/// </summary>
		/// <param name="c">The c.</param>
		private void ClearColor(Vector4 c) => GL.ClearColor(c.X, c.Y, c.Z, c.W);

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
