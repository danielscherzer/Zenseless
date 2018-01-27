using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Zenseless.ContentPipeline
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IResource{ITexture}" />
	public class ResourceTextureFile : IResource<ITexture>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceTextureFile"/> class.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public ResourceTextureFile(string filePath)
		{
			texture = TextureLoader.FromFile(filePath);
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
		public ITexture Value { get { return texture; } }

		/// <summary>
		/// Occurs when [change].
		/// </summary>
		public event EventHandler<ITexture> Change { add { } remove { } }

		/// <summary>
		/// The texture
		/// </summary>
		private ITexture texture;
	}
}
