using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.HLGL.IStateBool" />
	public class StateBoolGL : IStateBool
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StateBoolGL"/> class.
		/// </summary>
		/// <param name="capability">The capability.</param>
		public StateBoolGL(EnableCap capability)
		{
			this.capability = capability;
			UpdateGL();
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Zenseless.HLGL.IStateBool" /> is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled
		{
			get => enabled;
			set
			{
				if (value == enabled) return;
				enabled = value;
				UpdateGL();
			}
		}

		/// <summary>
		/// The enabled
		/// </summary>
		private bool enabled = false;
		/// <summary>
		/// The capability
		/// </summary>
		private readonly EnableCap capability;

		/// <summary>
		/// Updates the gl.
		/// </summary>
		private void UpdateGL()
		{
			if (enabled)
			{
				GL.Enable(capability);
			}
			else
			{
				GL.Disable(capability);
			}
		}
	}
}
