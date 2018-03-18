using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Example
{
	public class Visual
	{
        public static void DrawScreen(IGameState state)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			DrawGridLines(state.GridWidth, state.GridHeight);
			DrawGridCells(state);
		}

		private static void DrawGridCells(IGameState state)
		{
			GL.LineWidth(5.0f);
			var deltaX = 2.0f / (float)state.GridWidth;
			var deltaY = 2.0f / (float)state.GridHeight;
			var cell = new Box2D(0, 0, deltaX, deltaY);
			for (int u = 0; u < state.GridWidth; ++u)
			{
				for (int v = 0; v < state.GridHeight; ++v)
				{
					cell.MinX = deltaX * u - 1f;
					cell.MinY = deltaY * v - 1f;
					switch (state[u, v])
					{
						case FieldType.DIAMONT:
							DrawDiamont(cell);
							break;
						case FieldType.CROSS:
							DrawCross(cell);
							break;
						default: break;
					}
					
				}
			}
		}

		private static void DrawDiamont(IReadOnlyBox2D o)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(o.CenterX, o.MinY);
			GL.Vertex2(o.MaxX, o.CenterY);
			GL.Vertex2(o.CenterX, o.MaxY);
			GL.Vertex2(o.MinX, o.CenterY);
			GL.End();
		}

		private static void DrawCross(IReadOnlyBox2D o)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Vertex2(o.MinX, o.MinY);
			GL.Vertex2(o.MaxX, o.MaxY);

			GL.Vertex2(o.MaxX, o.MinY);
			GL.Vertex2(o.MinX, o.MaxY);
			GL.End();
		}

		private static void DrawGridLines(int width, int height)
		{
			GL.Color3(Color.White);
			GL.LineWidth(3.0f);
			var deltaX = 2.0f / (float)width;
			var deltaY = 2.0f / (float)height;
			GL.Begin(PrimitiveType.Lines);
			for (int i = 1; i < width; ++i)
			{
				var x = deltaX * i - 1.0f;
				GL.Vertex2(x, -1.0);
				GL.Vertex2(x, 1.0);
			}
			for (int i = 1; i < height; ++i)
			{
				var y = deltaY * i - 1.0f;
				GL.Vertex2(-1.0, y);
				GL.Vertex2(1.0, y);
			}
			GL.End();
		}
	}
}
