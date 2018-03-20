using Zenseless.HLGL;

namespace Heightfield
{
	public struct TextureBinding
	{
		public TextureBinding(string name, ITexture texture)
		{
			Name = name;
			Texture = texture;
		}

		public string Name { get; private set; }
		public ITexture Texture { get; private set; }
	}
}
