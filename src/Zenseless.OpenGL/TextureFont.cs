using Zenseless.Geometry;
using Zenseless.Base;
using OpenTK.Graphics.OpenGL;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// allows to print text were the individual characters come all from a single texture
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	public class TextureFont : Disposable
	{
		/// <summary>
		/// Create a new font that can be printed in OpenGL
		/// </summary>
		/// <param name="texture">texture containing a equally spaced grid of characters</param>
		/// <param name="charactersPerLine">number of characters per grid row</param>
		/// <param name="firstAsciiCode">ascii code of upper left most character in the grid</param>
		/// <param name="characterBoundingBoxWidth">bounding box width of each character cell, allows to zoom in/out of each character</param>
		/// <param name="characterBoundingBoxHeight">bounding box height of each character cell, allows to zoom in/out of each character</param>
		/// <param name="characterSpacing">how much to move to the right after drawing a single character</param>
		public TextureFont(ITexture texture, uint charactersPerLine = 16, byte firstAsciiCode = 0
			, float characterBoundingBoxWidth = 1.0f, float characterBoundingBoxHeight = 1.0f, float characterSpacing = 1.0f)
		{
			this.texFont = new SpriteSheet(texture, charactersPerLine, charactersPerLine, characterBoundingBoxWidth, characterBoundingBoxHeight);
			// Creating 256 Display Lists
			this.baseList = (uint)GL.GenLists(256);
			this.characterSpacing = characterSpacing;
			//foreach of the 256 possible characters create a a quad 
			//with texture coordinates and store it in a display list
			for (uint asciiCode = 0; asciiCode < 256; ++asciiCode)
			{
				GL.NewList((this.baseList + asciiCode), ListMode.Compile);

				var texCoords = texFont.CalcSpriteTexCoords(asciiCode - firstAsciiCode);
				Box2D.BOX01.DrawTexturedRect(texCoords);
				GL.Translate(characterSpacing, 0, 0);	// Move To The next character
				GL.EndList();
			}
		}

		/// <summary>
		/// Converts the string2 ASCII.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public byte[] ConvertString2Ascii(string text)
		{
			byte[] bytes = new byte[text.Length];
			uint pos = 0;
			foreach (char c in text)
			{
				bytes[pos] = (byte)c;
				++pos;
			}
			return bytes;
		}

		/// <summary>
		/// Prints the specified x position.
		/// </summary>
		/// <param name="xPos">The x position.</param>
		/// <param name="yPos">The y position.</param>
		/// <param name="zPos">The z position.</param>
		/// <param name="size">The size.</param>
		/// <param name="text">The text.</param>
		public void Print(float xPos, float yPos, float zPos, float size, string text)
		{
			GL.PushMatrix();
			GL.Translate(xPos, yPos, zPos);
			GL.Scale(size, size, size);
			var bytes = ConvertString2Ascii(text);
			texFont.Activate();
			PrintRawQuads(bytes);
			texFont.Deactivate();
			GL.PopMatrix();
		}

		/// <summary>
		/// Widthes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public float Width(string text, float size)
		{
			return text.Length * size * characterSpacing;
		}

		/// <summary>
		/// The base list
		/// </summary>
		private readonly uint baseList = 0; // Base Display List For The Font
											/// <summary>
											/// The tex font
											/// </summary>
		private readonly SpriteSheet texFont;
		/// <summary>
		/// The character spacing
		/// </summary>
		private readonly float characterSpacing;

		/// <summary>
		/// Prints the raw quads.
		/// </summary>
		/// <param name="text">The text.</param>
		private void PrintRawQuads(byte[] text)
		{
			if (text is null) return;
			GL.PushAttrib(AttribMask.ListBit);
			GL.PushMatrix();
			GL.ListBase(this.baseList);
			GL.CallLists(text.Length, ListNameType.UnsignedByte, text); // Write The Text To The Screen
			GL.PopMatrix();
			GL.PopAttrib();
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			GL.DeleteLists(this.baseList, 256); // Delete All 256 Display Lists
		}
	}
}
