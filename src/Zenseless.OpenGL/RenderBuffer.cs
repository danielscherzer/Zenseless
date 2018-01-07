using System;
using Zenseless.Base;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	public class RenderBuffer : Disposable
	{
		/// <summary>
		/// Gets the handle.
		/// </summary>
		/// <value>
		/// The handle.
		/// </value>
		public int Handle { get; private set; } = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="RenderBuffer"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public RenderBuffer(RenderbufferStorage type, int width, int height)
		{
			Handle = GL.GenRenderbuffer();
			Activate();
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, width, height);
			Deactivate();
		}

		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
		}

		/// <summary>
		/// Attaches the specified attachment point.
		/// </summary>
		/// <param name="attachmentPoint">The attachment point.</param>
		public void Attach(FramebufferAttachment attachmentPoint)
		{
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachmentPoint, RenderbufferTarget.Renderbuffer, Handle);
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			if (-1 != Handle) GL.DeleteRenderbuffer(Handle);
		}
	}
}
