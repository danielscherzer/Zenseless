using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.HLGL.IState" />
	public class StateActiveFboGL : IState
	{
		/// <summary>
		/// Gets or sets the fbo.
		/// </summary>
		/// <value>
		/// The fbo.
		/// </value>
		public FBO Fbo
		{
			get => fbo;
			set
			{
				if (ReferenceEquals(fbo, value)) return;
				fbo?.Deactivate();
				fbo = value;
				fbo?.Activate();
			}
		}

		/// <summary>
		/// The fbo
		/// </summary>
		private FBO fbo = null;
	}
}
