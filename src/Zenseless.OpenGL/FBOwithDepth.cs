using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.OpenGL.FBO" />
	public class FBOwithDepth : FBO
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FBOwithDepth"/> class.
		/// </summary>
		/// <param name="texture">The texture.</param>
		public FBOwithDepth(ITexture2D texture) : base(texture)
		{
			Activate();
			depth = new RenderBuffer(RenderbufferStorage.DepthComponent32, texture.Width, texture.Height);
			depth.Attach(FramebufferAttachment.DepthAttachment);
			Deactivate();
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			base.DisposeResources();
			depth.Dispose();
		}

		/// <summary>
		/// The depth
		/// </summary>
		private RenderBuffer depth;
	}
}