using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using Zenseless.Base;
using Zenseless.Geometry;

namespace ExampleBrowser
{
	using Line = Tuple<Vector2, Vector2>;

	[Export(typeof(IExample))]
	class AABBRotationExample : IExample
	{
		private GameTime time = new GameTime();
		private const float size = 0.7f;
		private Line stick = new Line(new Vector2(-size, -size), new Vector2(size, size));
		private Box2D stickAABB;

		//private AABBRotationExample(IRenderState renderState)
		//{
		//	renderState.Set(BlendStates.AlphaBlend);
		//	renderState.Set(BoolState<ILineSmoothState>.Enabled);

		//	GL.LineWidth(5.0f);
		//	renderState.Set(new ClearColorState(1, 1, 1, 1));
		//}

		private static Line RotateLine(Line stick, float rotationAngle)
		{
			var mtxRotation = Matrix2.CreateRotation(rotationAngle);
			Vector2 a;
			a.X = Vector2.Dot(mtxRotation.Column0, stick.Item1);
			a.Y = Vector2.Dot(mtxRotation.Column1, stick.Item1);
			Vector2 b;
			b.X = Vector2.Dot(mtxRotation.Column0, stick.Item2);
			b.Y = Vector2.Dot(mtxRotation.Column1, stick.Item2);
			return new Line(a, b);
		}

		private static void DrawLine(Line stick)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Vertex2(stick.Item1);
			GL.Vertex2(stick.Item2);
			GL.End();
		}

		private static void DrawAABB(IReadOnlyBox2D rect)
		{
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex2(rect.MinX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
		}

		public void Render()
		{
			time.NewFrame();
			float angle = -time.DeltaTime * 0.6f;

			stick = RotateLine(stick, angle);
			var minX = Math.Min(stick.Item1.X, stick.Item2.X);
			var maxX = Math.Max(stick.Item1.X, stick.Item2.X);
			var minY = Math.Min(stick.Item1.Y, stick.Item2.Y);
			var maxY = Math.Max(stick.Item1.Y, stick.Item2.Y);
			stickAABB = Box2DExtensions.CreateFromMinMax(minX, minY, maxX, maxY);

			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Color3(Color.CornflowerBlue);
			DrawLine(stick);

			GL.Color3(Color.YellowGreen);
			DrawAABB(stickAABB);

			GL.Color3(Color.Black);
			DrawAABB(Box2DExtensions.CreateFromCenterSize(0, 0, 0.02f, 0.02f));
		}
	}
}