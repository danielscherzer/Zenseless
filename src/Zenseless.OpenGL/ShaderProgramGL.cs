namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Patterns;
	using Zenseless.HLGL;
	using ShaderType = HLGL.ShaderType;

	/// <summary>
	/// OpenGL shader program class
	/// </summary>
	/// <seealso cref="Disposable" />
	/// <seealso cref="IShaderProgram" />
	public class ShaderProgramGL : Disposable, IShaderProgram
	{
		/// <summary>
		/// Gets the program identifier.
		/// </summary>
		/// <value>
		/// The program identifier.
		/// </value>
		public int ProgramID { get; private set; } = 0;
		/// <summary>
		/// Gets the shader log.
		/// </summary>
		/// <value>
		/// The log.
		/// </value>
		public string Log { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderProgramGL" /> class.
		/// </summary>
		public ShaderProgramGL()
		{
			ProgramID = GL.CreateProgram();
			if (0 == ProgramID) throw new ShaderException($"Could not create {nameof(ShaderProgramGL)} instance");
		}

		/// <summary>
		/// Attaches the specified shader.
		/// </summary>
		/// <param name="shader">The shader.</param>
		public void Attach(ShaderGL shader)
		{
			GL.AttachShader(ProgramID, shader.ShaderID);
			shaders.Add(shader);
		}

		/// <summary>
		/// Compiles the specified shader source code.
		/// </summary>
		/// <param name="shaderSourceCode">The shader source code.</param>
		/// <param name="type">The type.</param>
		/// <exception cref="ShaderCompileException"></exception>
		public void Compile(string shaderSourceCode, ShaderType type)
		{
			var shader = new ShaderGL(type);
			if (!shader.Compile(shaderSourceCode))
			{
				var e = new ShaderCompileException(type, shader.Log, shaderSourceCode);
				shader.Dispose();
				throw e;
			}
			Attach(shader);
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
			int GetResourceIndex(ProgramInterface type)
			{
				return GL.GetProgramResourceIndex(ProgramID, type, name);
			}

			switch (resourceType)
			{
				case ShaderResourceType.Attribute: return GL.GetAttribLocation(ProgramID, name);
				case ShaderResourceType.UniformBuffer: return GetResourceIndex(ProgramInterface.UniformBlock);
				case ShaderResourceType.RWBuffer: return GetResourceIndex(ProgramInterface.ShaderStorageBlock);
				case ShaderResourceType.Uniform: return GL.GetUniformLocation(ProgramID, name);
				default: throw new ArgumentOutOfRangeException("Unknown ShaderResourceType");
			}
		}

		/// <summary>
		/// Links all compiled shaders to a shader program and deletes them.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ShaderLinkException"></exception>
		public bool Link()
		{
			GL.LinkProgram(ProgramID);
			GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out int status_code);
			RemoveShaders();
			Log = GL.GetProgramInfoLog(ProgramID);
			return 1 == status_code;
		}

		/// <summary>
		/// Set int Uniform on active shader.
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="value">The value to set.</param>
		/// <exception cref="NotImplementedException"></exception>
		public void Uniform(string name, int value)
		{
			var loc = GetResourceLocation(ShaderResourceType.Uniform, name);
			GL.ProgramUniform1(ProgramID, loc, value);
		}

		/// <summary>
		/// Set float Uniform on active shader.
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="value">The value to set.</param>
		public void Uniform(string name, float value)
		{
			var loc = GetResourceLocation(ShaderResourceType.Uniform, name);
			GL.ProgramUniform1(ProgramID, loc, value);
		}

		/// <summary>
		/// Set Vector2 Uniform on active shader.
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		public void Uniform(string name, in Vector2 vector)
		{
			var loc = GetResourceLocation(ShaderResourceType.Uniform, name);
			GL.ProgramUniform2(ProgramID, loc, vector.X, vector.Y);
		}

		/// <summary>
		/// Set Vector3 Uniform on active shader.
		/// </summary>
		/// <param name="location">The shader variable location.</param>
		/// <param name="vector">The vector.</param>
		public void Uniform(int location, in Vector3 vector)
		{
			GL.ProgramUniform3(ProgramID, location, vector.X, vector.Y, vector.Z);
		}

		/// <summary>
		/// Set Vector3 Uniform on active shader.
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		public void Uniform(string name, in Vector3 vector)
		{
			var loc = GetResourceLocation(ShaderResourceType.Uniform, name);
			Uniform(loc, vector);
		}

		/// <summary>
		/// Set Vector4 Uniform on active shader.
		/// </summary>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="vector">The vector.</param>
		public void Uniform(string name, in Vector4 vector)
		{
			var loc = GetResourceLocation(ShaderResourceType.Uniform, name);
			GL.ProgramUniform4(ProgramID, loc, vector.X, vector.Y, vector.Z, vector.W);
		}

		/// <summary>
		/// Set matrix uniforms on active shader.
		/// </summary>
		/// <param name="location">The shader variable location.</param>
		/// <param name="matrix">The input matrix.</param>
		/// <param name="transpose">if set to <c>true</c> the matrix is transposed.</param>
		public void Uniform(int location, in Matrix4x4 matrix, bool transpose = false)
		{
			// Matrix4x4 has internally a transposed memory layout
			//unsafe //TODO: check unsafe appveyor problems
			//{
			//	fixed (float* matrix_ptr = &matrix.M11)
			//	{
			//		GL.ProgramUniformMatrix4(ProgramID, location, 1, !transpose, matrix_ptr);
			//	}
			//}
			var m = new OpenTK.Matrix4(matrix.M11, matrix.M12, matrix.M13, matrix.M14,
					matrix.M21, matrix.M22, matrix.M23, matrix.M24,
					matrix.M31, matrix.M32, matrix.M33, matrix.M34,
					matrix.M41, matrix.M42, matrix.M43, matrix.M44);

			GL.ProgramUniformMatrix4(ProgramID, location, !transpose, ref m);
		}

		/// <summary>
		/// Set matrix uniforms on shader on active shader.
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
		private List<ShaderGL> shaders = new List<ShaderGL>();

		/// <summary>
		/// Removes all attached shaders.
		/// </summary>
		public void RemoveShaders()
		{
			foreach (var shader in shaders)
			{
				GL.DetachShader(ProgramID, shader.ShaderID);
				shader.Dispose();
			}
			shaders.Clear();
		}
	}
}
