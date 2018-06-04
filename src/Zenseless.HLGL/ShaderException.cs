namespace Zenseless.HLGL
{
	using System;

	/// <summary>
	/// The base exception class for shaders.
	/// </summary>
	/// <seealso cref="Exception" />
	public class ShaderException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public ShaderException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
		public ShaderException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	/// <summary>
	///  Occurs when linking is failing.
	/// </summary>
	/// <seealso cref="ShaderException" />
	public class ShaderLinkException : ShaderException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderLinkException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public ShaderLinkException(string message) : base(message)
		{
		}
	}

	/// <summary>
	/// Occurs when compilation is failing.
	/// </summary>
	/// <seealso cref="ShaderException" />
	public class ShaderCompileException : ShaderException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderCompileException"/> class.
		/// </summary>
		/// <param name="shaderType">Type of the shader.</param>
		/// <param name="message">The message.</param>
		/// <param name="shaderSourceCode">The shader source code.</param>
		public ShaderCompileException(ShaderType shaderType, string message, string shaderSourceCode) : base(message)
		{
			ShaderType = shaderType;
			ShaderSourceCode = shaderSourceCode.Replace("\r", string.Empty);
		}

		/// <summary>
		/// Gets the shader code.
		/// </summary>
		/// <value>
		/// The shader code.
		/// </value>
		public string ShaderSourceCode { get; }

		/// <summary>
		/// Gets the type of the shader.
		/// </summary>
		/// <value>
		/// The type of the shader.
		/// </value>
		public ShaderType ShaderType { get; }
	}
}
