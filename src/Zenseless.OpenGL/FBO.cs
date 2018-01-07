using Zenseless.Base;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL;
using System;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="System.Exception" />
	[Serializable]
	public class FBOException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FBOException" /> class.
		/// </summary>
		/// <param name="msg">The error msg.</param>
		public FBOException(string msg) : base(msg) { }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	public class FBO : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FBO"/> class.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <exception cref="Zenseless.OpenGL.FBOException">
		/// Texture is null
		/// or
		/// </exception>
		public FBO(ITexture2D texture)
		{
			if (texture is null) throw new FBOException("Texture is null");
			this.texture = texture;

			// Create an FBO object
			GL.GenFramebuffers(1, out m_FBOHandle);
			Activate();

			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture.ID, 0);

			string status = GetStatusMessage();
			Deactivate();
			if (!string.IsNullOrEmpty(status))
			{
				throw new FBOException(status);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsActive {  get { return currentFrameBufferHandle == m_FBOHandle; } }
		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		public ITexture2D Texture { get { return texture; } }

		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			GL.PushAttrib(AttribMask.ViewportBit);
			lastFBO = currentFrameBufferHandle;
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_FBOHandle);
			GL.Viewport(0, 0, Texture.Width, Texture.Height);
			currentFrameBufferHandle = m_FBOHandle;
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, lastFBO);
			GL.PopAttrib();
			currentFrameBufferHandle = lastFBO;
		}

		/// <summary>
		/// The texture
		/// </summary>
		private ITexture2D texture;
		/// <summary>
		/// The m fbo handle
		/// </summary>
		private uint m_FBOHandle = 0;
		/// <summary>
		/// The last fbo
		/// </summary>
		private uint lastFBO = 0;
		/// <summary>
		/// The current frame buffer handle
		/// </summary>
		private static uint currentFrameBufferHandle = 0;

		/// <summary>
		/// Gets the status message.
		/// </summary>
		/// <returns></returns>
		private string GetStatusMessage()
		{
			switch (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer))
			{
				case FramebufferErrorCode.FramebufferComplete: return string.Empty;
				case FramebufferErrorCode.FramebufferIncompleteAttachment: return "One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.";
				case FramebufferErrorCode.FramebufferIncompleteMissingAttachment: return "There are no attachments.";
				case FramebufferErrorCode.FramebufferIncompleteDimensionsExt: return "Attachments are of different size. All attachments must have the same width and height.";
				case FramebufferErrorCode.FramebufferIncompleteFormatsExt: return "The color attachments have different format. All color attachments must have the same format.";
				case FramebufferErrorCode.FramebufferIncompleteDrawBuffer: return "An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.";
				case FramebufferErrorCode.FramebufferIncompleteReadBuffer: return "The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.";
				case FramebufferErrorCode.FramebufferUnsupported: return "This particular FBO configuration is not supported by the implementation.";
				default: return "Status unknown. (yes, this is really bad.)";
			}
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			GL.DeleteFramebuffers(1, ref m_FBOHandle);
		}
	}
}
