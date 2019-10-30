﻿namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class Visual
	{
		public Visual(IRenderState renderState, IContentLoader contentLoader)
		{
			texSand = contentLoader.Load<ITexture2D>("dryCrackedSand");
			texTruck = contentLoader.Load<ITexture2D>("truck");
			shaderRoad = contentLoader.Load<IShaderProgram>("shader.*");
			shaderTruck = contentLoader.Load<IShaderProgram>("colorTexture.*");
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.PointSmooth);
			//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
		}

		public void Render(IReadOnlyList<Vector2> points, int selectedPoint, float truckPos)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Ortho(-windowAspect, windowAspect, -1, 1, 0, 1);

			shaderRoad.Activate();
			texSand.Activate();

			DrawLineStrip(points);

			texSand.Deactivate();
			shaderRoad.Deactivate();

			GL.Color3(OpenTK.Color.Red);
			GL.PointSize(15.0f);
			DrawPoints(points);

			if(-1 != selectedPoint)
			{
				GL.Color3(OpenTK.Color.Blue);
				GL.PointSize(25.0f);
				DrawPoint(points[selectedPoint]);
			}
			var pos = CubicHermiteSpline.CatmullRomSpline(points, truckPos);
			GL.Color3(OpenTK.Color.White);
			//GL.Color3(Color.Green);
			//GL.PointSize(25.0f);
			shaderTruck.Activate();
			texTruck.Activate();
			DrawTools.DrawTexturedRect(Box2DExtensions.CreateFromCenterSize(pos.X, pos.Y, 0.1f, 0.1f), Box2D.BOX01);
			texTruck.Deactivate();
			shaderTruck.Deactivate();
			//DrawPoint(pos);
		}

		internal Vector2 ConvertWindowCoords(in Vector2 coordWindow)
		{
			var v = coordWindow;
			v.X *= windowAspect;
			return v;
		}

		internal void Resize(int width, int height)
		{
			windowAspect = width / (float)height;
		}

		private readonly ITexture texSand;
		private readonly ITexture texTruck;
		private readonly IShaderProgram shaderRoad;
		private readonly IShaderProgram shaderTruck;
		private float windowAspect;

		private void DrawPoint(in Vector2 point)
		{
			GL.Begin(PrimitiveType.Points);
			GL.Vertex2(point.X, point.Y);
			GL.End();
		}

		private void DrawPoints(IEnumerable<Vector2> points)
		{
			GL.Begin(PrimitiveType.Points);
			foreach (var point in points)
			{
				GL.Vertex2(point.X, point.Y);
			}
			GL.End();
		}

		private void DrawLineStrip(IReadOnlyList<Vector2> points)
		{
			GL.Begin(PrimitiveType.LineStripAdjacency);
			var first = points[0];
			GL.Vertex2(first.X, first.Y);
			foreach (var point in points)
			{
				GL.Vertex2(point.X, point.Y);
			}
			var last = points[points.Count - 1];
			GL.Vertex2(last.X, last.Y);
			GL.End();
		}
	}
}
