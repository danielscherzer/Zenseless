using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class SpriteRenderer
	{
		public SpriteRenderer(IRenderState renderState) 
		{
			//for transparency in textures we use blending
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		public void AddSprite(string name, int layer, IReadOnlyBox2D renderBounds, NamedStream namedStream)
		{
			var texture = GetTexture(namedStream);
			if (!layers.ContainsKey(layer))
			{
				layers.Add(layer, new Layer());
			}
			var myBoundsCopy = new Box2D(renderBounds); //make a copy, otherwise we reference outside data
			layers[layer].Add(texture, myBoundsCopy);
			AddNamedSprite(name, myBoundsCopy);
		}

		public void Resize(int width, int height)
		{
			aspect = width / (float)height;
		}

		public void Render(IReadOnlyBox2D bounds)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			var fitBox = Box2DExtensions.CreateContainingBox(bounds.SizeX, bounds.SizeY, aspect);
			GL.Ortho(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0, 1);
			GL.MatrixMode(MatrixMode.Modelview);
			foreach (var layer in layers)
			{
				layer.Value.Draw();
			}
		}

		public void UpdateSprites(string name, float x, float y)
		{
			var sprites = FindNamedSprites(name);
			foreach (var sprite in sprites)
			{
				//TODO: do some hierarchical transform, otherwise all hierarchical sprites have same position
				sprite.CenterX = x;
				sprite.CenterY = y;
			}
		}

		private class Layer
		{
			public void Add(ITexture tex, IReadOnlyBox2D bounds)
			{
				if (!textures.ContainsKey(tex))
				{
					textures.Add(tex, new List<IReadOnlyBox2D>());
				}
				textures[tex].Add(bounds);
			}

			public void Draw()
			{
				foreach(var tex in textures)
				{
					tex.Key.Activate();
					foreach(var box in tex.Value)
					{
						box.DrawTexturedRect(Box2D.BOX01);
					}
					tex.Key.Deactivate();
				}
			}

			private Dictionary<ITexture, List<IReadOnlyBox2D>> textures = new Dictionary<ITexture, List<IReadOnlyBox2D>>();
		}

		private SortedDictionary<int, Layer> layers = new SortedDictionary<int, Layer>();
		private Dictionary<string, ITexture> textures = new Dictionary<string, ITexture>();
		private Dictionary<string, List<Box2D>> namedSprites = new Dictionary<string, List<Box2D>>();
		private float aspect = 1f;

		private void AddNamedSprite(string name, Box2D bounds)
		{
			if (string.IsNullOrWhiteSpace(name)) return;
			var n = name.ToLowerInvariant();
			if (!namedSprites.ContainsKey(n))
			{
				namedSprites.Add(n, new List<Box2D>());
			}
			namedSprites[n].Add(bounds);
		}

		private IEnumerable<Box2D> FindNamedSprites(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return null;
			var n = name.ToLowerInvariant();
			if (namedSprites.TryGetValue(n, out List<Box2D> sprites))
			{
				return sprites;
			}
			return null;
		}

		private ITexture GetTexture(NamedStream namedStream)
		{
			if (!textures.TryGetValue(namedStream.Name, out ITexture texture))
			{
				//TextureLoaderImaging.FromStream(namedStream.Stream);
				var bitmap = new Bitmap(namedStream.Stream);
				texture = TextureLoaderDrawing.FromBitmap(bitmap);
				texture.Filter = TextureFilterMode.Mipmap;
				texture.WrapFunction = TextureWrapFunction.ClampToEdge;
				textures.Add(namedStream.Name, texture);
			}
			return texture;
		}
	}
}
