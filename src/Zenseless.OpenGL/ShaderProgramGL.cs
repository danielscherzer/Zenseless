using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Zenseless.Base;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	using ShaderType = HLGL.ShaderType;
	using TKShaderType = OpenTK.Graphics.OpenGL4.ShaderType;

	/// <summary>
	/// OpenGL shader program class
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	/// <seealso cref="Zenseless.HLGL.IShaderProgram" />
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
		/// </exception>
		public void Link()
		{
			try
			{
				GL.LinkProgram(ProgramID);
			}
			catch (Exception)
			{
				throw new ShaderException("Unknown Link error!", string.Empty);
			}
			GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out int status_code);
			if (1 != status_code)
			{
				throw new ShaderException("Error linking shader", GL.GetProgramInfoLog(ProgramID));
			}
			IsLinked = true;
			RemoveShaders();
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
