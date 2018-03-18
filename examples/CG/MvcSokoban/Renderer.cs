using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace MvcSokoban
{
	public class Renderer : IRenderer
	{
		public Renderer(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(BlendStates.AlphaBlend); //for transparency in textures we use blending

			tileSet.Add(ElementType.Floor, contentLoader.Load<ITexture2D>("GroundGravel_Grass"));
			tileSet.Add(ElementType.Man, contentLoader.Load<ITexture2D>("Character4"));
			tileSet.Add(ElementType.Box, contentLoader.Load<ITexture2D>("Crate_Brown"));
			tileSet.Add(ElementType.Goal, contentLoader.Load<ITexture2D>("EndPoint_Red"));
			tileSet.Add(ElementType.ManOnGoal, contentLoader.Load<ITexture2D>("EndPointCharacter"));
			tileSet.Add(ElementType.BoxOnGoal, contentLoader.Load<ITexture2D>("EndPointCrate_Brown"));
			tileSet.Add(ElementType.Wall, contentLoader.Load<ITexture2D>("Wall_Beige"));

			font = new FontGL(contentLoader);
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void DrawLevelState(ILevel level, Color tint)
		{
			GL.Color3(tint);
			GL.LoadIdentity();
			var fitBox = Box2DExtensions.CreateContainingBox(level.Width, level.Height, windowAspect);
			GL.Ortho(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0.0, 1.0);
			for (int x = 0; x < level.Width; ++x)
			{
				for (int y = 0; y < level.Height; ++y)
				{
					var tile = new Box2D(x, y, 1.0f, 1.0f);
					var element = level.GetElement(x, y);
					var tex = tileSet[element];
					tex.Activate();
					tile.DrawTexturedRect(Box2D.BOX01);
					tex.Deactivate();
				}
			}
		}

		public void Print(string message, float size, TextAlignment alignment = TextAlignment.Left)
		{
			font.Print(message, size, alignment);
		}

		public void ResizeWindow(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			font.ResizeWindow(width, height);
			windowAspect = width / (float)height;
		}

		private FontGL font;
		private Dictionary<ElementType, ITexture> tileSet = new Dictionary<ElementType, ITexture>();
		private float windowAspect;
	}
}
