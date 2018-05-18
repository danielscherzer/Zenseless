namespace Zenseless.HLGL
{
	/// <summary>
	/// State structure for setting the point size in the shader via gl_PointSize.
	/// </summary>
	public struct ShaderPointSize : IState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderPointSize"/> structure.
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public ShaderPointSize(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ShaderPointSize"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; }
	}
}
