using Zenseless.Base;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Gl Texture class that allows loading from a file.
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	/// <seealso cref="Zenseless.HLGL.ITexture" />
	public class Texture : Disposable, ITexture
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Texture" /> class.
		/// </summary>
		/// <param name="target">The target.</param>
		public Texture(TextureTarget target = TextureTarget.Texture2D)
		{
			//generate one texture and put its ID number into the "m_uTextureID" variable
			GL.GenTextures(1, out m_uTextureID);
			//GL.CreateTextures(target, 1, out m_uTextureID); //DSA not supported by intel
			Target = target;
		}

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		public TextureTarget Target { get; }

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public uint ID { get { return m_uTextureID; } }

		/// <summary>
		/// Gets or sets the filter.
		/// </summary>
		/// <value>
		/// The filter.
		/// </value>
		public TextureFilterMode Filter
		{
			get => filterMode;
			set => SetFilter(value);
		}

		/// <summary>
		/// Gets or sets the wrap function.
		/// </summary>
		/// <value>
		/// The wrap function.
		/// </value>
		public TextureWrapFunction WrapFunction
		{
			get => wrapFunction;
			set => SetWrapMode(value);
		}

		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			GL.BindTexture(Target, m_uTextureID);
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.BindTexture(Target, 0);
		}

		/// <summary>
		/// Converts the specified components.
		/// </summary>
		/// <param name="components">The components.</param>
		/// <param name="floatingPoint">if set to <c>true</c> [floating point].</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Invalid Format only 1-4 components allowed</exception>
		public static PixelInternalFormat Convert(byte components = 4, bool floatingPoint = false)
		{
			switch (components)
			{
				case 1: return floatingPoint ? PixelInternalFormat.R32f : PixelInternalFormat.R8;
				case 2: return floatingPoint ? PixelInternalFormat.Rg32f : PixelInternalFormat.Rg8;
				case 3: return floatingPoint ? PixelInternalFormat.Rgb32f : PixelInternalFormat.Rgb8;
				case 4: return floatingPoint ? PixelInternalFormat.Rgba32f : PixelInternalFormat.Rgba8;
			}
			throw new ArgumentOutOfRangeException("Invalid Format only 1-4 components allowed");
		}

		/// <summary>
		/// Converts the specified components.
		/// </summary>
		/// <param name="components">The components.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Invalid Format only 1-4 components allowed</exception>
		public static PixelFormat Convert(byte components = 4)
		{
			switch (components)
			{
				case 1: return PixelFormat.Red;
				case 2: return PixelFormat.Rg;
				case 3: return PixelFormat.Rgb;
				case 4: return PixelFormat.Rgba;
			}
			throw new ArgumentOutOfRangeException("Invalid Format only 1-4 components allowed");
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			GL.DeleteTexture(m_uTextureID);
		}

		/// <summary>
		/// The m u texture identifier
		/// </summary>
		private readonly uint m_uTextureID = 0;
		/// <summary>
		/// The filter mode
		/// </summary>
		private TextureFilterMode filterMode;
		/// <summary>
		/// The wrap function
		/// </summary>
		private TextureWrapFunction wrapFunction;

		/// <summary>
		/// Converts the wrap function.
		/// </summary>
		/// <param name="wrapFunc">The wrap function.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Unknown wrap function</exception>
		private static int ConvertWrapFunction(TextureWrapFunction wrapFunc)
		{
			switch (wrapFunc)
			{
				case TextureWrapFunction.ClampToBorder: return (int)TextureWrapMode.ClampToBorder;
				case TextureWrapFunction.ClampToEdge: return (int)TextureWrapMode.ClampToEdge;
				case TextureWrapFunction.Repeat: return (int)TextureWrapMode.Repeat;
				case TextureWrapFunction.MirroredRepeat: return (int)TextureWrapMode.MirroredRepeat;
				default: throw new ArgumentOutOfRangeException("Unknown wrap function");
			}
		}

		/// <summary>
		/// Sets the filter.
		/// </summary>
		/// <param name="filter">The filter.</param>
		private void SetFilter(TextureFilterMode filter)
		{
			//case TextureFilterMode.Nearest
			var magFilter = (int)TextureMagFilter.Nearest;
			var minFilter = (int)TextureMinFilter.Nearest;
			var mipmap = 0;
			switch (filter)
			{
				case TextureFilterMode.Linear:
					magFilter = (int)TextureMagFilter.Linear;
					minFilter = (int)TextureMinFilter.Linear;
					break;
				case TextureFilterMode.Mipmap:
					magFilter = (int)TextureMagFilter.Linear;
					minFilter = (int)TextureMinFilter.LinearMipmapLinear;
					mipmap = 1;
					break;
			}
			Activate();
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, magFilter);
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, minFilter);
			GL.TexParameter(Target, TextureParameterName.GenerateMipmap, mipmap);
			Deactivate();
			filterMode = filter;
		}

		/// <summary>
		/// Sets the wrap mode.
		/// </summary>
		/// <param name="wrapFunc">The wrap function.</param>
		private void SetWrapMode(TextureWrapFunction wrapFunc)
		{
			var mode = ConvertWrapFunction(wrapFunc);
			Activate();
			GL.TexParameter(Target, TextureParameterName.TextureWrapS, mode);
			GL.TexParameter(Target, TextureParameterName.TextureWrapT, mode);
			GL.TexParameter(Target, TextureParameterName.TextureWrapR, mode);
			Deactivate();
			wrapFunction = wrapFunc;
		}
	}
}
