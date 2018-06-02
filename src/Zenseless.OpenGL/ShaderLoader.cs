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
		/// The exception data key string name that contains the file name
		/// </summary>
		public const string ExceptionDataFileName = "fileName";

		/// <summary>
		/// Reads the contents of a file into a string
		/// </summary>
		/// <param name="shaderFile">path to the shader file</param>
		/// <param name="testCompileInclude">should includes be compiled (for error checking) before being pasted into the including shader</param>
		/// <returns>
		/// string with contents of shaderFile
		/// </returns>
		/// <exception cref="FileNotFoundException">
		/// Could not find shader file '" + shaderFile + "'
		/// or
		/// Could not find include-file '" + sIncludeFileName + "' for shader '" + shaderFile + "'.
		/// </exception>
		internal static string ShaderStringFromFileWithIncludes(string shaderFile, bool testCompileInclude)
		{
			string sShader = null;
			if (!File.Exists(shaderFile))
			{
				throw new FileNotFoundException("Could not find shader file '" + shaderFile + "'");
			}
			sShader = File.ReadAllText(shaderFile);

			//handle includes
			string sCurrentPath = Path.GetDirectoryName(shaderFile) + Path.DirectorySeparatorChar; // get path to current shader
			string sName = Path.GetFileName(shaderFile);
			//split into lines
			var lines = sShader.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			var pattern = @"^\s*#include\s+""([^""]+)"""; //match everything inside " except " so we get shortest ".+" match 
			int lineNr = 1;
			foreach (var line in lines)
			{
				// Search for include pattern (e.g. #include raycast.glsl) (nested not supported)
				foreach (Match match in Regex.Matches(line, pattern, RegexOptions.Singleline))
				{
					string sFullMatch = match.Value;
					string sIncludeFileName = match.Groups[1].ToString(); // get the filename to include
					string sIncludePath = sCurrentPath + sIncludeFileName; // build path to file

					if (!File.Exists(sIncludePath))
					{
						throw new FileNotFoundException("Could not find include-file '" + sIncludeFileName + "' for shader '" + shaderFile + "'.");
					}
					string sIncludeShd = File.ReadAllText(sIncludePath); // read include as string
					if (testCompileInclude)
					{
						TestCompile(sIncludePath, sIncludeShd);
					}
					sIncludeShd += Environment.NewLine + "#line " + lineNr.ToString() + Environment.NewLine;
					sShader = sShader.Replace(sFullMatch, sIncludeShd); // replace #include with actual include
				}
				++lineNr;
			}
			return sShader;
		}

		internal static void TestCompile(string shaderName, string shaderCode)
		{
			using (var shader = new ShaderProgramGL())
			{
				try
				{
					shader.Compile(shaderCode, ShaderType.FragmentShader); //test compile include shader
				}
				catch (ShaderCompileException e)
				{
					var ce = new ShaderCompileException(e.ShaderType, $"include compile '{shaderName}'", e.ShaderLog, shaderCode);
					ce.Data.Add(ExceptionDataFileName, shaderName);
					throw ce;
				}
			}
		}

		/// <summary>
		/// Extracts the name of the shader file.
		/// </summary>
		/// <param name="e">The <seealso cref="ShaderException"/></param>
		/// <returns></returns>
		public static string GetFileName(this ShaderException e)
		{
			if (e.Data.Contains(ExceptionDataFileName))
			{
				return e.Data[ExceptionDataFileName] as string;
			}
			return string.Empty;
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

			var lines = shaderCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
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
