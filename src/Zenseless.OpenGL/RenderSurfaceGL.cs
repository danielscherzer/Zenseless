using Zenseless.Base;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Zenseless.OpenGL
{
	//todo: move all gl classes into a manager class that handles dispose; do not use gl classes directly
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	/// <seealso cref="Zenseless.HLGL.IRenderSurface" />
	public class RenderSurfaceGL : Disposable, IRenderSurface
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RenderSurfaceGL"/> class.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="hasDepthBuffer">if set to <c>true</c> [has depth buffer].</param>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		public RenderSurfaceGL(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false): this(hasDepthBuffer)
		{
			var tex = OpenGL.Texture2dGL.Create(width, height, components, floatingPoint);
			if (hasDepthBuffer)
			{
				fbo = new FBOwithDepth(tex);
			}
			else
			{
				fbo = new FBO(tex);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RenderSurfaceGL"/> class.
		/// </summary>
		/// <param name="hasDepthBuffer">if set to <c>true</c> [has depth buffer].</param>
		public RenderSurfaceGL(bool hasDepthBuffer = false)
		{
			if (context is null)
			{
				context = new RenderContextGL();
				//context.StateManager.Get<IStateTyped<Vector4>, States.IClearColor>().Value = new Vector4(0, .3f, .7f, 1);
			}

			if (hasDepthBuffer)
			{
				actionClear = () => GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			}
			else
			{
				actionClear = () => GL.Clear(ClearBufferMask.ColorBufferBit);
			}
		}

		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		public ITexture Texture { get { return fbo?.Texture; } }

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			context.StateManager.Get<StateActiveFboGL, StateActiveFboGL>().Fbo = fbo;
			actionClear();
		}

		/// <summary>
		/// Draws the specified configuration.
		/// </summary>
		/// <param name="config">The configuration.</param>
		public void Draw(IDrawConfiguration config)
		{
			context.StateManager.Get<StateActiveFboGL, StateActiveFboGL>().Fbo = fbo;
			config.Draw(context);
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			if (!(fbo is null)) fbo.Dispose();
		}

		/// <summary>
		/// The fbo
		/// </summary>
		private FBO fbo = null;
		/// <summary>
		/// The action clear
		/// </summary>
		private Action actionClear = null;
		/// <summary>
		/// The context
		/// </summary>
		private static RenderContextGL context = null;
	}
}
