namespace Zenseless.HLGL
{
	/// <summary>
	/// A class for shader binding meshes with shader and textures
	/// </summary>
	public struct TextureBinding
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextureBinding"/> structure.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="texture">The texture.</param>
		public TextureBinding(string name, ITexture texture)
		{
			Name = name;
			Texture = texture;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		public ITexture Texture { get; private set; }
	}
}
