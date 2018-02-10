using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public enum ShaderType
	{
		/// <summary>
		/// The fragment shader
		/// </summary>
		FragmentShader,
		/// <summary>
		/// The vertex shader
		/// </summary>
		VertexShader,
		/// <summary>
		/// The geometry shader
		/// </summary>
		GeometryShader,
		/// <summary>
		/// The tess evaluation shader
		/// </summary>
		TessEvaluationShader,
		/// <summary>
		/// The tess control shader
		/// </summary>
		TessControlShader,
		/// <summary>
		/// The compute shader
		/// </summary>
		ComputeShader
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ShaderResourceType
	{
		/// <summary>
		/// The uniform
		/// </summary>
		Uniform,
		/// <summary>
		/// The attribute
		/// </summary>
		Attribute,
		/// <summary>
		/// The uniform buffer
		/// </summary>
		UniformBuffer,
		/// <summary>
		/// The rw buffer
		/// </summary>
		RWBuffer
	}

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface IShaderProgram : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this instance is linked.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is linked; otherwise, <c>false</c>.
		/// </value>
		bool IsLinked { get; }
		/// <summary>
		/// Gets the last log.
		/// </summary>
		/// <value>
		/// The last log.
		/// </value>
		string LastLog { get; }
		/// <summary>
		/// Gets the program identifier.
		/// </summary>
		/// <value>
		/// The program identifier.
		/// </value>
		int ProgramID { get; }

		/// <summary>
		/// Activates this instance.
		/// </summary>
		void Activate();
		/// <summary>
		/// Compiles the specified s shader.
		/// </summary>
		/// <param name="sShader">The s shader.</param>
		/// <param name="type">The type.</param>
		void Compile(string sShader, ShaderType type);
		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		void Deactivate();
		/// <summary>
		/// Gets the resource location.
		/// </summary>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		int GetResourceLocation(ShaderResourceType resourceType, string name);
		/// <summary>
		/// Links this instance.
		/// </summary>
		void Link();
	}
}