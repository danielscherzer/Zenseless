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
		public IShaderProgram ShaderProgram
		{
			get => shaderProgram;
			set
			{
				if (ReferenceEquals(shaderProgram, value)) return;
				shaderProgram?.Deactivate();
				shaderProgram = value;
				shaderProgram?.Activate();
			}
		}

		/// <summary>
		/// The shader
		/// </summary>
		private IShaderProgram shaderProgram = null;
	}
}
