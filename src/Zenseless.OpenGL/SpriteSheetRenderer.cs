using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	public class SpriteSheetRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpriteSheetRenderer"/> class.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="spriteSheet">The sprite sheet.</param>
		public SpriteSheetRenderer(ITexture texture, SpriteSheet spriteSheet)
		{
			SpriteSheet = spriteSheet;
			Texture = texture;
			Texture.Filter = TextureFilterMode.Mipmap;
		}
		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			Texture.Activate();
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			Texture.Deactivate();
		}

		/// <summary>
		/// Draws the specified rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="id">The identifier.</param>
		public void Draw(IReadOnlyBox2D rectangle, uint id)
		{
			var texCoords = SpriteSheet.CalcSpriteTexCoords(id);
			Texture.Activate();
			rectangle.DrawTexturedRect(texCoords);
			Texture.Deactivate();

		}

		/// <summary>
		/// Gets the sprite sheet.
		/// </summary>
		/// <value>
		/// The sprite sheet.
		/// </value>
		public SpriteSheet SpriteSheet { get; private set; }
		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		public ITexture Texture { get; private set; }
	}
}
