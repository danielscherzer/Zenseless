using Zenseless.HLGL;
using System;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	public class DoubleBufferedFBO
	{
		/// <summary>
		/// Gets the active fbo.
		/// </summary>
		/// <value>
		/// The active fbo.
		/// </value>
		public FBO ActiveFBO { get; private set; }
		/// <summary>
		/// Gets the inactive fbo.
		/// </summary>
		/// <value>
		/// The inactive fbo.
		/// </value>
		public FBO InactiveFBO { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DoubleBufferedFBO"/> class.
		/// </summary>
		/// <param name="creator">The creator.</param>
		public DoubleBufferedFBO(Func<ITexture2D> creator)
		{
			ActiveFBO = new FBO(creator());
			InactiveFBO = new FBO(creator());
		}

		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			ActiveFBO.Activate();
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			ActiveFBO.Deactivate();
		}

		/// <summary>
		/// Swaps the buffers.
		/// </summary>
		public void SwapBuffers()
		{
			var temp = ActiveFBO;
			ActiveFBO = InactiveFBO;
			InactiveFBO = ActiveFBO;
		}
	}
}
