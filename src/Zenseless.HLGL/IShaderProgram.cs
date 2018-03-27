namespace Zenseless.HLGL
{
	using System;
	using System.Numerics;

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
	/// <seealso cref="IDisposable" />
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
		/// Compiles the specified shader type from the source code provided.
		/// </summary>
		/// <param name="sShader">The shader source code.</param>
		/// <param name="type">The shader type.</param>
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

		/// <summary>
		/// Set int Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="value">The value to set.</param>
		void Uniform(string name, int value);

		/// <summary>
		/// Set float Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="value">The value to set.</param>
		void Uniform(string name, float value);

		/// <summary>
		/// Set Vector2 Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		void Uniform(string name, in Vector2 vector);

		/// <summary>
		/// Set Vector3 Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		void Uniform(string name, in Vector3 vector);

		/// <summary>
		/// Set Vector4 Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		void Uniform(string name, in Vector4 vector);

		/// <summary>
		/// Set matrix uniforms on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="matrix">The input matrix.</param>
		/// <param name="transpose">if set to <c>true</c> the matrix is transposed.</param>
		void Uniform(string name, in Matrix4x4 matrix, bool transpose = false);
	}
}