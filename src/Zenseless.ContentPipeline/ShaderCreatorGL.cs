using Zenseless.HLGL;
using System;
using System.Collections.Generic;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="ICreator{IShader}" />
	public class ShaderCreatorGL : ICreator<IShader>
	{
		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		public TypedHandle<IShader> Create()
		{
			var shader = new Shader();
			shaders.Add(shader.ProgramID, shader);
			return new TypedHandle<IShader>(shader.ProgramID);
		}

		/// <summary>
		/// Gets the specified handle.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Invalid Handle id given</exception>
		public IShader Get(TypedHandle<IShader> handle)
		{
			if(shaders.TryGetValue(handle.ID, out IShader shader))
			{
				return shader;
			}
			throw new ArgumentException("Invalid Handle id given");
		}

		/// <summary>
		/// The shaders
		/// </summary>
		private Dictionary<int, IShader> shaders = new Dictionary<int, IShader>();
	}
}
