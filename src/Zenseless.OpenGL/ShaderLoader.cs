namespace Zenseless.OpenGL
{
	using System;
	using System.Text.RegularExpressions;
	using Zenseless.HLGL;

	/// <summary>
	/// Contains methods for loading Glsl Shader programs from files, streams or strings
	/// </summary>
	public static class ShaderLoader
	{
		/// <summary>
		/// Creates from strings.
		/// </summary>
		/// <param name="vertexShaderCode">The vertex shader code.</param>
		/// <param name="fragmentShaderCode">The fragment shader code.</param>
		/// <returns></returns>
		public static ShaderProgramGL CreateFromStrings(string vertexShaderCode, string fragmentShaderCode)
		{
			var vertexShader = CreateShader(ShaderType.VertexShader, vertexShaderCode);
			var fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderCode);
			var shaderProgram = new ShaderProgramGL();
			shaderProgram.Attach(vertexShader);
			shaderProgram.Attach(fragmentShader);
			if (!shaderProgram.Link())
			{
				var e = new ShaderLinkException(shaderProgram.Log);
				shaderProgram.Dispose();
				throw e;
			}
			return shaderProgram;
		}

		private static ShaderGL CreateShader(ShaderType shaderType, string shaderCode)
		{
			var shader = new ShaderGL(shaderType);
			if (!shader.Compile(shaderCode))
			{
				var e = shader.CreateException(shaderCode);
				shader.Dispose();
				throw e;
			}
			return shader;
		}

		/// <summary>
		/// Creates a <seealso cref="ShaderCompileException"/>.
		/// </summary>
		/// <param name="shader">The shader.</param>
		/// <param name="shaderCode">The shader code.</param>
		/// <returns></returns>
		public static ShaderCompileException CreateException(this ShaderGL shader, string shaderCode)
		{
			return new ShaderCompileException(shader.ShaderType, shader.Log, shaderCode);
		}
	}
}
