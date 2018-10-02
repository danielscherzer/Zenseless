namespace Zenseless.HLGL
{
	/// <summary>
	/// Interface for a shader uniform
	/// </summary>
	public interface IUniform
	{
		/// <summary>
		/// Gets the name of the uniform.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		string Name { get; }

		/// <summary>
		/// Updates the uniform for the given shader program.
		/// </summary>
		/// <param name="shaderProgram">The shader program.</param>
		void Update(IShaderProgram shaderProgram);
	}
}
