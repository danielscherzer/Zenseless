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
		/// Gets the shader log.
		/// </summary>
		/// <value>
		/// The shader log.
		/// </value>
		public string ShaderLog { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderException" /> class.
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="log">The shader log</param>
		public ShaderException(string message, string log) : base(message)
		{
			ShaderLog = log;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="ShaderException" />
	public class ShaderCompileException : ShaderException
	{
		/// <summary>
		/// Gets the type of the shader.
		/// </summary>
		/// <value>
		/// The type of the shader.
		/// </value>
		public ShaderType ShaderType { get; private set; }
		/// <summary>
		/// Gets the shader code.
		/// </summary>
		/// <value>
		/// The shader code.
		/// </value>
		public string ShaderCode { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderException" /> class.
		/// </summary>
		/// <param name="shaderType">The type of the shader</param>
		/// <param name="msg">The error msg.</param>
		/// <param name="log">The shader log</param>
		/// <param name="shaderCode">The source code of the shader</param>
		public ShaderCompileException(ShaderType shaderType, string msg, string log, string shaderCode) : base(msg, log)
		{
			ShaderType = shaderType;
			ShaderCode = shaderCode.Replace("\r", string.Empty);
		}
	}
}
