using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.HLGL.IState" />
	public class StateActiveShaderGL : IState
	{
		/// <summary>
		/// Gets or sets the shader.
		/// </summary>
		/// <value>
		/// The shader.
		/// </value>
		public IShader Shader
		{
			get => shader;
			set
			{
				if (ReferenceEquals(shader, value)) return;
				shader?.Deactivate();
				shader = value;
				shader?.Activate();
			}
		}

		/// <summary>
		/// The shader
		/// </summary>
		private IShader shader = null;
	}
}
