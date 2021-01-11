namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.Collections.Generic;
	using Zenseless.Patterns;
	using Zenseless.HLGL;

	/// <summary>
	/// Frame buffer object class that handles rendering to texture(s).
	/// </summary>
	/// <seealso cref="Disposable" />
	public class FBO : Disposable, IRenderSurface
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FBO"/> class.
		/// </summary>
		/// <param name="texture">The texture to draw to. The FBO will try to dispose the texture when the FBO is disposed.</param>
		/// <exception cref="FBOException">
		/// Given texture is null or texture dimensions do not match primary texture
		/// </exception>
		public FBO(ITexture2D texture)
		{
			// Create an FBO object
			GL.CreateFramebuffers(1, out handle);
			// attach first texture
			Attach(texture);
		}

		/// <summary>
		/// Attaches the specified texture. The FBO will try to dispose the texture when the FBO is disposed.
		/// </summary>
		/// <param name="texture">The texture to attach.</param>
		/// <exception cref="FBOException">
		/// Given texture is null or texture dimensions do not match primary texture
		/// </exception>
		public void Attach(ITexture2D texture)
		{
			if (texture is null) throw new ArgumentNullException(nameof(texture));
			if (Texture is null)
			{
				Texture = texture;
			}
			else
			{
				if (Texture.Width != texture.Width || Texture.Height != texture.Height)
					throw new ArgumentException($"Given Texture dimension ({texture.Width},{texture.Height}) " +
						$"do not match primary texture ({Texture.Width},{Texture.Height})");
			}
			attachments.Add(texture);
			GL.NamedFramebufferTexture(handle, AttachmentFromID(attachments.Count - 1), texture.ID, 0);
			CheckFramebufferStatus();

			var drawBuffers = new List<DrawBuffersEnum>();
			for (int i = 0; i < attachments.Count; ++i)
			{
				drawBuffers.Add(DrawBuffersEnum.ColorAttachment0 + i);
			}
			this.drawBuffers = drawBuffers.ToArray();
		}

		/// <summary>
		/// Gets the first color texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		public ITexture2D Texture { get; private set; }
		/// <summary>
		/// Gets the list of attached textures.
		/// </summary>
		/// <value>
		/// The list of textures.
		/// </value>
		public IReadOnlyList<ITexture2D> Textures => attachments;

		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			GL.PushAttrib(AttribMask.ViewportBit);
			lastFBO = currentFrameBufferHandle;
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
			GL.Viewport(0, 0, Texture.Width, Texture.Height);
			currentFrameBufferHandle = handle;
			GL.DrawBuffers(drawBuffers.Length, drawBuffers);
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.DrawBuffers(1, drawBuffers); //TODO: not a complete reverse
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, lastFBO);
			GL.PopAttrib(); //TODO: deprecated, but needed by view port
			currentFrameBufferHandle = lastFBO;
		}

		/// <summary>
		/// Execute the specified action on the render surface.
		/// </summary>
		/// <param name="draw">The code to draw.</param>
		public void Draw(Action draw)
		{
			Activate();
			draw();
			Deactivate();
		}

		private readonly uint handle = 0;
		private uint lastFBO = 0;
		private static uint currentFrameBufferHandle = 0;
		private readonly List<ITexture2D> attachments = new List<ITexture2D>();
		private DrawBuffersEnum[] drawBuffers = new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0 };

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			foreach (var tex in attachments) tex.Dispose();
			GL.DeleteFramebuffer(handle);
		}

		private static FramebufferAttachment AttachmentFromID(int id) => FramebufferAttachment.ColorAttachment0 + id;

		private void CheckFramebufferStatus()
		{
			string status = GetStatusMessage();
			if (status is null) return;
			throw new FBOException(status);
		}

		private string GetStatusMessage()
		{
			return (GL.CheckNamedFramebufferStatus(handle, FramebufferTarget.Framebuffer)) switch
			{
				FramebufferStatus.FramebufferComplete => null,
				FramebufferStatus.FramebufferIncompleteAttachment => "One or more attachment points are not frame buffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.",
				FramebufferStatus.FramebufferIncompleteDrawBuffer => "An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.",
				FramebufferStatus.FramebufferIncompleteLayerTargets => "Frame buffer Incomplete Layer Targets",
				FramebufferStatus.FramebufferIncompleteMissingAttachment => "There are no attachments.",
				FramebufferStatus.FramebufferIncompleteMultisample => "Frame buffer incomplete multi sample",
				FramebufferStatus.FramebufferIncompleteReadBuffer => "The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.",
				FramebufferStatus.FramebufferUndefined => "Frame buffer Undefined",
				FramebufferStatus.FramebufferUnsupported => "This particular FBO configuration is not supported by the implementation.",
				_ => "Status unknown. (yes, this is really bad.)",
			};
		}
	}
}
