using Zenseless.HLGL;
using System;
using System.Collections.Generic;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="ICreator{IShader}" />
	public class ShaderCreatorGL : ICreator<IShaderProgram>
	{
		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		public TypedHandle<IShaderProgram> Create()
		{
			var shader = new ShaderProgramGL();
			shaders.Add(shader.ProgramID, shader);
			return new TypedHandle<IShaderProgram>(shader.ProgramID);
		}

		/// <summary>
		/// Gets the specified handle.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Invalid Handle id given</exception>
		public IShaderProgram Get(TypedHandle<IShaderProgram> handle)
		{
			if(shaders.TryGetValue(handle.ID, out IShaderProgram shaderProgram))
			{
				return shaderProgram;
			}
			throw new ArgumentException("Invalid Handle id given");
		}

		/// <summary>
		/// The shaders
		/// </summary>
		private Dictionary<int, IShaderProgram> shaders = new Dictionary<int, IShaderProgram>();
	}
}
