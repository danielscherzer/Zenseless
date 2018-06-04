namespace Zenseless.HLGL
{
	using System;

	/// <summary>
	/// Is thrown when the shader content importer threw a <seealso cref="ShaderException"/> (contained in inner exception)
	/// </summary>
	/// <seealso cref="Exception" />
	public class NamedShaderException : ShaderException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NamedShaderException"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="innerException">The inner exception.</param>
		public NamedShaderException(string name, ShaderException innerException) : base(name, innerException)
		{
			Name = name;
			InnerException = innerException;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; }
		/// <summary>
		/// Gets the <see cref="ShaderException" /> instance that caused the current exception.
		/// </summary>
		public new ShaderException InnerException { get; }
	}
}
