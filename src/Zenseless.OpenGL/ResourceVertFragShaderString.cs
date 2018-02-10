using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Zenseless.ContentPipeline
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IResource{IShader}" />
	public class ResourceVertFragShaderString : IResource<IShaderProgram>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceVertFragShaderString"/> class.
		/// </summary>
		/// <param name="sVertex">The s vertex.</param>
		/// <param name="sFragment">The s fragment.</param>
		public ResourceVertFragShaderString(string sVertex, string sFragment)
		{
			shaderProgram = ShaderLoader.FromStrings(sVertex, sFragment);
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
		public IShaderProgram Value { get { return shaderProgram; } }

		/// <summary>
		/// Occurs when [change].
		/// </summary>
		public event EventHandler<IShaderProgram> Change { add { } remove { } }

		/// <summary>
		/// The shader
		/// </summary>
		private IShaderProgram shaderProgram;
	}
}