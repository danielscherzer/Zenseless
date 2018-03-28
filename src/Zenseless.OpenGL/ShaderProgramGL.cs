namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Base;
	using Zenseless.HLGL;
	using ShaderType = HLGL.ShaderType;
	using TKShaderType = OpenTK.Graphics.OpenGL4.ShaderType;

	/// <summary>
	/// OpenGL shader program class
	/// </summary>
	/// <seealso cref="Disposable" />
	/// <seealso cref="IShaderProgram" />
	/// TODO: create Shader classes to compile individual (fragment, vertex, ...) shaders
	public class ShaderProgramGL : Disposable, IShaderProgram
	{
		/// <summary>
		/// Gets a value indicating whether this instance is linked.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is linked; otherwise, <c>false</c>.
		/// </value>
		public bool IsLinked { get; private set; } = false;

		/// <summary>
		/// Gets the last log.
		/// </summary>
		/// <value>
		/// The last log.
		/// </value>
		public string LastLog { get; private set; }

		/// <summary>
		/// Gets the program identifier.
		/// </summary>
		/// <value>
		/// The program identifier.
		/// </value>
		public int ProgramID { get; private set; } = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderProgramGL" /> class.
		/// </summary>
		public ShaderProgramGL()
		{
			ProgramID = GL.CreateProgram();
		}

		/// <summary>
		/// Compiles the specified s shader.
		/// </summary>
		/// <param name="sShader">The s shader.</param>
		/// <param name="type">The type.</param>
		/// <exception cref="ShaderCompileException">
		/// Could not create " + type.ToString() + " object
		/// or
		/// Error compiling  " + type.ToString()
		/// </exception>
		public void Compile(string sShader, ShaderType type)
		{
			IsLinked = false;
			int shaderObject = GL.CreateShader(ConvertType(type));
			if (0 == shaderObject) throw new ShaderCompileException(type, "Could not create " + type.ToString() + " object", string.Empty, sShader);
			// Compile vertex shader
			GL.ShaderSource(shaderObject, sShader);
			GL.CompileShader(shaderObject);
			GL.GetShader(shaderObject, ShaderParameter.CompileStatus, out int status_code);
			LastLog = GL.GetShaderInfoLog(shaderObject);
			if (1 != status_code)
			{
				GL.DeleteShader(shaderObject);
				throw new ShaderCompileException(type, "Error compiling  " + type.ToString(), LastLog, sShader);
			}
			GL.AttachShader(ProgramID, shaderObject);
			shaderIDs.Add(shaderObject);
		}

		/// <summary>
		/// Begins this shader use.
		/// </summary>
		public void Activate()
		{
			GL.UseProgram(ProgramID);
		}

		/// <summary>
		/// Ends this shader use.
		/// </summary>
		public void Deactivate()
		{
			GL.UseProgram(0);
		}

		/// <summary>
		/// Gets the resource location.
		/// </summary>
		/// <param name="resourceType">Type of the resource.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Unknown ShaderResourceType</exception>
		public int GetResourceLocation(ShaderResourceType resourceType, string name)
		{
			switch(resourceType)
			{
				case ShaderResourceType.Attribute: return GL.GetAttribLocation(ProgramID, name);
				case ShaderResourceType.UniformBuffer: return GetResourceIndex(name, ProgramInterface.UniformBlock);
				case ShaderResourceType.RWBuffer: return GetResourceIndex(name, ProgramInterface.ShaderStorageBlock);
				case ShaderResourceType.Uniform: return GetResourceIndex(name, ProgramInterface.Uniform);
				default: throw new ArgumentOutOfRangeException("Unknown ShaderResourceType");
			}
		}

		/// <summary>
		/// Links all compiled shaders to a shader program and deletes them.
		/// </summary>
		/// <exception cref="ShaderException">
		/// Unknown Link error!
		/// or
		/// Error linking shader
		/// This will also delete all compiled shaders
		/// </exception>
		public void Link()
		{
			try
			{
				GL.LinkProgram(ProgramID);
				GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out int status_code);
				if (1 != status_code)
				{
					throw new ShaderException("Error linking shader", GL.GetProgramInfoLog(ProgramID));
				}
				IsLinked = true;
			}
			catch (Exception)
			{
				throw new ShaderException("Unknown Link error!", string.Empty);
			}
			finally
			{
				RemoveShaders();
			}
		}

		/// <summary>
		/// Set int Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="value">The value to set.</param>
		/// <exception cref="NotImplementedException"></exception>
		public void Uniform(string name, int value)
		{
			GL.Uniform1(GetResourceLocation(ShaderResourceType.Uniform, name), value);
		}

		/// <summary>
		/// Set float Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="value">The value to set.</param>
		public void Uniform(string name, float value)
		{
			GL.Uniform1(GetResourceLocation(ShaderResourceType.Uniform, name), value);
		}

		/// <summary>
		/// Set Vector2 Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		public void Uniform(string name, in Vector2 vector)
		{
			GL.Uniform2(GetResourceLocation(ShaderResourceType.Uniform, name), vector.X, vector.Y);
		}

		/// <summary>
		/// Set Vector3 Uniform.
		/// </summary>
		/// <param name="location">The shader variable location.</param>
		/// <param name="vector">The vector.</param>
		public static void Uniform(int location, in Vector3 vector)
		{
			GL.Uniform3(location, vector.X, vector.Y, vector.Z);
		}

		/// <summary>
		/// Set Vector3 Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		public void Uniform(string name, in Vector3 vector)
		{
			Uniform(GetResourceLocation(ShaderResourceType.Uniform, name), vector);
		}

		/// <summary>
		/// Set Vector4 Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		public void Uniform(string name, in Vector4 vector)
		{
			GL.Uniform4(GetResourceLocation(ShaderResourceType.Uniform, name), vector.X, vector.Y, vector.Z, vector.W);
		}

		/// <summary>
		/// Set matrix uniforms.
		/// </summary>
		/// <param name="location">The shader variable location.</param>
		/// <param name="matrix">The input matrix.</param>
		/// <param name="transpose">if set to <c>true</c> the matrix is transposed.</param>
		public static void Uniform(int location, in Matrix4x4 matrix, bool transpose = false)
		{
			// Matrix4x4 has internally a transposed memory layout
			//unsafe //TODO: check unsafe appveyor problems
			//{
			//	fixed (float* matrix_ptr = &matrix.M11)
			//	{
			//		GL.UniformMatrix4(location, 1, !transpose, matrix_ptr);
			//	}
			//}
			var m = new OpenTK.Matrix4(matrix.M11, matrix.M12, matrix.M13, matrix.M14,
					matrix.M21, matrix.M22, matrix.M23, matrix.M24,
					matrix.M31, matrix.M32, matrix.M33, matrix.M34,
					matrix.M41, matrix.M42, matrix.M43, matrix.M44);

			GL.UniformMatrix4(location, !transpose, ref m);
		}

		/// <summary>
		/// Set matrix uniforms on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="matrix">The input matrix.</param>
		/// <param name="transpose">if set to <c>true</c> the matrix is transposed.</param>
		public void Uniform(string name, in Matrix4x4 matrix, bool transpose = false)
		{
			Uniform(GetResourceLocation(ShaderResourceType.Uniform, name), matrix, transpose);
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			if (0 != ProgramID)
			{
				GL.DeleteProgram(ProgramID);
			}
		}

		/// <summary>
		/// The shader ids used for linking
		/// </summary>
		private List<int> shaderIDs = new List<int>();

		/// <summary>
		/// Converts the shader type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Unknown Shader type</exception>
		private TKShaderType ConvertType(ShaderType type)
		{
			switch(type)
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

		/// <summary>
		/// Gets the index of the resource.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private int GetResourceIndex(string name, ProgramInterface type)
		{
			return GL.GetProgramResourceIndex(ProgramID, type, name);
		}

		/// <summary>
		/// Removes the shaders.
		/// </summary>
		private void RemoveShaders()
		{
			foreach (int id in shaderIDs)
			{
				GL.DetachShader(ProgramID, id);
				GL.DeleteShader(id);
			}
			shaderIDs.Clear();
		}
	}
}
