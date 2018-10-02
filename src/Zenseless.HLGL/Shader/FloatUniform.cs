namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IUniform" />
	public struct FloatUniform : IUniform
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FloatUniform"/> struct.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public FloatUniform(string name, float value)
		{
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Gets the name of the uniform.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; }
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public float Value { get; }

		/// <summary>
		/// Updates the specified program.
		/// </summary>
		/// <param name="program">The program.</param>
		public void Update(IShaderProgram program)
		{
			program.Uniform(Name, Value);
		}
	}
}
