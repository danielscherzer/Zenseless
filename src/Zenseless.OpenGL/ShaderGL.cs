namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.Patterns;
	using Zenseless.HLGL;
	using ShaderType = HLGL.ShaderType;
	using TKShaderType = OpenTK.Graphics.OpenGL4.ShaderType;
	using System;

	/// <summary>
	/// OpenGL shader class
	/// </summary>
	/// <seealso cref="Disposable" />
	public class ShaderGL : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderGL"/> class.
		/// </summary>
		/// <param name="shaderType">Type of the shader.</param>
		/// <exception cref="ShaderException">Could not create " + shaderType.ToString() + " instance.</exception>
		public ShaderGL(ShaderType shaderType)
		{
			ShaderID = GL.CreateShader(ConvertType(shaderType));
			if (0 == ShaderID) throw new ShaderException("Could not create " + shaderType.ToString() + " instance.");
			ShaderType = shaderType;
		}

		/// <summary>
		/// Compiles the specified source code.
		/// </summary>
		/// <param name="sourceCode">The source code.</param>
		/// <returns></returns>
		public bool Compile(string sourceCode)
		{
			GL.ShaderSource(ShaderID, sourceCode);
			GL.CompileShader(ShaderID);
			GL.GetShader(ShaderID, ShaderParameter.CompileStatus, out int status_code);
			return 1 == status_code;
		}

		/// <summary>
		/// Gets the last shader [compilation] log.
		/// </summary>
		/// <value>
		/// The log string.
		/// </value>
		public string Log => GL.GetShaderInfoLog(ShaderID);

		/// <summary>
		/// Gets the OpenGL shader identifier.
		/// </summary>
		/// <value>
		/// The shader identifier.
		/// </value>
		public int ShaderID { get; }
		
		/// <summary>
		/// Gets the type of the shader.
		/// </summary>
		/// <value>
		/// The type of the shader.
		/// </value>
		public ShaderType ShaderType { get; }

		/// <summary>
		/// Will be called from the default Dispose method.
		/// Implementers should dispose all their resources her.
		/// </summary>
		protected override void DisposeResources()
		{
			GL.DeleteShader(ShaderID);
		}

		private TKShaderType ConvertType(ShaderType type)
		{
			switch (type)
			{
				case ShaderType.ComputeShader: return TKShaderType.ComputeShader;
				case ShaderType.FragmentShader: return TKShaderType.FragmentShader;
				case ShaderType.GeometryShader: return TKShaderType.GeometryShader;
				case ShaderType.TessControlShader: return TKShaderType.TessControlShader;
				case ShaderType.TessEvaluationShader: return TKShaderType.TessEvaluationShader;
				case ShaderType.VertexShader: return TKShaderType.VertexShader;
				default: throw new ArgumentOutOfRangeException("Unknown Shader type");
			}
		}
	}
}
