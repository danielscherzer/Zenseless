using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Zenseless.Application
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IResource{IShader}" />
	public class ResourceVertFragShaderFile : IResource<IShader>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceVertFragShaderFile"/> class.
		/// </summary>
		/// <param name="sVertexShdFile_">The s vertex SHD file.</param>
		/// <param name="sFragmentShdFile_">The s fragment SHD file.</param>
		public ResourceVertFragShaderFile(string sVertexShdFile_, string sFragmentShdFile_)
		{
			shader = ShaderLoader.FromFiles(sVertexShdFile_, sFragmentShdFile_);
		}

		/// <summary>
		/// Gets a value indicating whether this instance is value created.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is value created; otherwise, <c>false</c>.
		/// </value>
		public bool IsValueCreated { get { return !(shader is null); } }

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
