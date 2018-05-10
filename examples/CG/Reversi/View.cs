using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Reversi
{
	public class View
	{
		public View(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(new LineWidth(5f));
			texWhite = contentLoader.Load<ITexture2D>("white");
			texBlack = contentLoader.Load<ITexture2D>("black");
			texTable = contentLoader.Load<ITexture2D>("pool_table");
			font = new TextureFont(contentLoader.Load<ITexture2D>("Fire 2"), 10, 32, 1.0f, 0.9f, 0.5f);
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		public void Resize(IGameState gameState, int width, int height)
		{
			var fitBox = Box2DExtensions.CreateContainingBox(gameState.GridWidth, gameState.GridHeight, width / (float)height);
			toClipSpace = Matrix4.CreateOrthographicOffCenter(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0f, 1f);
		}

		public Point CalcGridPos(Vector2 coord)
		{
			//calculate the grid coordinates
			var pos = new Vector4(coord.X, coord.Y, 0.0f, 1.0f);
			var fromClipSpace = toClipSpace.Inverted();
			var gridPos = Vector4.Transform(pos, fromClipSpace);
			return new Point((int)Math.Floor(gridPos.X), (int)Math.Floor(gridPos.Y));
		}

		public void Render(IGameState gameState)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref toClipSpace);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawField(gameState);
		}

		public void PrintMessage(string message)
		{
			GL.MatrixMode(MatrixMode.Projection);
			var mtxAspect = Matrix4.CreateOrthographic(1, 1, 0, 1);
			GL.LoadMatrix(ref mtxAspect);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.Color3(Color.White);
			var size = 0.1f;
			font.Print(-0.5f * font.Width(message, size), 0, 0, size, message);
		}

		private Matrix4 toClipSpace = new Matrix4();
		private TextureFont font;
		private ITexture texWhite;
		private ITexture texBlack;
		private ITexture texTable;

		private void DrawField(IGameState gameState)
		{
			//background
			GL.Color3(Color.White);
			var field = new Box2D(0, 0, gameState.GridWidth, gameState.GridHeight);
			texTable.Activate();
			field.DrawTexturedRect(new Box2D(0, 0, 8, 8));
			texTable.Deactivate();
			//grid
			GL.Color3(Color.Black);
			GL.Begin(PrimitiveType.Lines);
			for (int i = 0; i <= gameState.GridWidth; ++i)
			{
				GL.Vertex2(i, 0.0);
				GL.Vertex2(i, gameState.GridHeight);
			}
			for (int i = 0; i <= gameState.GridHeight; ++i)
			{
				GL.Vertex2(0.0, i);
				GL.Vertex2(gameState.GridWidth, i);
			}
			GL.End();
			//chips
			GL.Color3(Color.White);
			for (int x = 0; x < gameState.GridWidth; ++x)
			{
				for (int y = 0; y < gameState.GridHeight; ++y)
				{
					var type = gameState[x, y];
					if (FieldType.EMPTY == type) continue;
					DrawSprite(x + 0.5f, y + 0.5f, FieldType.BLACK == type ? texBlack : texWhite, 0.45f);
				}
			}
			GL.Color3(Color.Blue);
			DrawSelection(gameState.LastMoveX, gameState.LastMoveY);
		}

		static void DrawSelection(int x_, int y_)
		{
			float x = x_ + 0.5f;
			float y = y_ + 0.5f;
			float radius = 0.48f;
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(x - radius, y - radius);
			GL.Vertex2(x + radius, y - radius);
			GL.Vertex2(x + radius, y + radius);
			GL.Vertex2(x - radius, y + radius);
			GL.End();
		}

		static void DrawSprite(float x, float y, ITexture tex, float radius = 0.5f, float repeat = 1.0f)
		{
			tex.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(x - radius, y - radius);
			GL.TexCoord2(repeat, 0.0f); GL.Vertex2(x + radius, y - radius);
			GL.TexCoord2(repeat, repeat); GL.Vertex2(x + radius, y + radius);
			GL.TexCoord2(0.0f, repeat); GL.Vertex2(x - radius, y + radius);
			GL.End();
			tex.Deactivate();
		}
	}
}
