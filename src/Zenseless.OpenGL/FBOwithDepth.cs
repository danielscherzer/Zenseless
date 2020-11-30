namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.HLGL;

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="FBO" />
	public class FBOwithDepth : FBO
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FBOwithDepth"/> class.
		/// </summary>
		/// <param name="texture">The texture.</param>
		public FBOwithDepth(ITexture2D texture) : base(texture)
		{
			Draw(() =>
			{
				depth = new RenderBuffer(RenderbufferStorage.DepthComponent32, texture.Width, texture.Height);
				depth.Attach(FramebufferAttachment.DepthAttachment);
			});
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			base.DisposeResources();
			depth.Dispose();
		}

		private RenderBuffer depth;
	}
}