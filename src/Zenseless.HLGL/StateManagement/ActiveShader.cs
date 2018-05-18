namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for the active shader program.
	/// </summary>
	public struct ActiveShader : IState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveShader"/> structure.
		/// </summary>
		/// <param name="shaderProgram">The shader program.</param>
		public ActiveShader(IShaderProgram shaderProgram)
		{
			ShaderProgram = shaderProgram;
		}

		/// <summary>
		/// Gets the shader program.
		/// </summary>
		/// <value>
		/// The shader program.
		/// </value>
		public IShaderProgram ShaderProgram { get; }
	}
}
