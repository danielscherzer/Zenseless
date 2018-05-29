using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace MiniGalaxyBirds
{
	public class Renderer : IRenderer
	{
		public Renderer()
		{
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		public IEnumerable<IDrawable> Drawables => drawables;

		public void Register(string type, ITexture texture)
		{
			registeredTypes[type] = texture;
		}

		public void RegisterFont(TextureFont textureFont)
		{
			font = textureFont;
		}

		public IDrawable CreateDrawable(string type, IReadOnlyBox2D frame)
		{
			if (registeredTypes.TryGetValue(type, out ITexture tex))
			{
				IDrawable drawable = new Sprite(tex, frame);
				drawables.Add(drawable);
				return drawable;
			}
			throw new Exception("Unregisterd type " + type.ToString());
		}

		public IDrawable CreateDrawable(string type, IReadOnlyBox2D frame, IAnimation animation)
		{
			if (registeredTypes.TryGetValue(type, out ITexture tex))
			{
				IDrawable drawable = new AnimatedSprite(tex, frame, animation);
				drawables.Add(drawable);
				return drawable;
			}
			throw new Exception("Unregisterd type " + type.ToString());
		}

		public void DeleteDrawable(IDrawable drawable)
		{
			drawables.Remove(drawable);
		}

		public void Print(float x, float y, float z, float size, string text)
		{
			if (font is null)
			{
				throw new Exception("No font registered!");
			}
			font.Print(x, y, z, size, text);
		}

		private readonly Dictionary<string, ITexture> registeredTypes = new Dictionary<string, ITexture>();
		private readonly HashSet<IDrawable> drawables = new HashSet<IDrawable>();

		private TextureFont font = null;
	}
}
