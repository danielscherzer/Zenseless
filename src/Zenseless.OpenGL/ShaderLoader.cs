namespace Zenseless.OpenGL
{
	using System;
	using System.IO;
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

		/// <summary>
		/// Searches for #include statements in the shader code and replaces them by the code in the include resource.
		/// </summary>
		/// <param name="shaderCode">The shader code.</param>
		/// <param name="GetIncludeCode">Functor that will be called with the include path and that should return the shader code.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">GetIncludeCode</exception>
		public static string ResolveIncludes(string shaderCode, Func<string, string> GetIncludeCode)
		{
			if (GetIncludeCode == null) throw new ArgumentNullException(nameof(GetIncludeCode));

			var lines = shaderCode.Split(new[] { '\n' }, StringSplitOptions.None); //if UNIX style line endings still working so do not use Envirnoment.NewLine
			var pattern = @"^\s*#include\s+""([^""]+)"""; //match everything inside " except " so we get shortest ".+" match 
			int lineNr = 1;
			foreach (var line in lines)
			{
				// Search for include pattern (e.g. #include raycast.glsl) (nested not supported)
				foreach (Match match in Regex.Matches(line, pattern, RegexOptions.Singleline))
				{
					var sFullMatch = match.Value;
					var includeName = match.Groups[1].ToString(); // get the include
					var includeCode = GetIncludeCode(includeName) + Environment.NewLine + "#line " + lineNr.ToString() + Environment.NewLine;
					shaderCode = shaderCode.Replace(sFullMatch, includeCode); // replace #include with actual include
				}
				++lineNr;
			}
			return shaderCode;
		}
	}
}
