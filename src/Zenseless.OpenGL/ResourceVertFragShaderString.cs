using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Zenseless.Application
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IResource{IShader}" />
	public class ResourceVertFragShaderString : IResource<IShader>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceVertFragShaderString"/> class.
		/// </summary>
		/// <param name="sVertex">The s vertex.</param>
		/// <param name="sFragment">The s fragment.</param>
		public ResourceVertFragShaderString(string sVertex, string sFragment)
		{
			shader = ShaderLoader.FromStrings(sVertex, sFragment);
		}

		/// <summary>
		/// Gets a value indicating whether this instance is value created.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is value created; otherwise, <c>false</c>.
		/// </value>
		public bool IsValueCreated { get { return true; } }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public IShader Value { get { return shader; } }

		/// <summary>
		/// Occurs when [change].
		/// </summary>
		public event EventHandler<IShader> Change { add { } remove { } }

		/// <summary>
		/// The shader
		/// </summary>
		private IShader shader;
	}
}