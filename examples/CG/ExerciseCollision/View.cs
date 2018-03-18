using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using Zenseless.Base;
using Zenseless.Geometry;
using Zenseless.OpenGL;

namespace Example
{
	public class View
	{
		public View()
		{
			//create an asteroid shape out of a circle with perturbed corner vertices
			ShapeBuilder.Circle((float x, float y) => asteroid.Add(new Vector2(x, y)), 0f, 0f, .5f, 20); //create circle
			var rndGenerator = new Random(12);
			float Rnd() => .9f + (float)rndGenerator.NextDouble() * .1f; //random number in range [0.9, 1]
			for (int i = 0; i < asteroid.Count; ++i)
			{
				asteroid[i] = asteroid[i] * Rnd(); //scale circle vertices with random values;
			}
			GL.LineWidth(3.0f);
		}

		public void ClearScreen()
		{
			time.NewFrame();

			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			GL.Ortho(-aspect, aspect, -1, 1, 0, 1); //keep correct scaling on window resize
		}

		public void DrawShape(IReadOnlyCircle boundingCircle)
		{
			GL.Color3(Color.LightSlateGray);
			DrawObstacle(boundingCircle);
			GL.Color3(Color.White);
			DrawCollisionOutline(boundingCircle);
		}

		public void Resize(int width, int height)
		{
			aspect = width / (float)height;
		}

		private List<Vector2> asteroid = new List<Vector2>();
		private float aspect = 1f;
		private GameTime time = new GameTime();

		private static void DrawCollisionOutline(IReadOnlyCircle boundingCircle)
		{
			DrawTools.DrawCircle(boundingCircle.CenterX, boundingCircle.CenterY, boundingCircle.Radius, 20, false);
		}

		private void DrawObstacle(IReadOnlyCircle boundingCircle)
		{
			GL.PushMatrix();
			{
				// we use only one instance of asteroid vertices, so we have to 
				// move those to the position and scale those to the size of each asteroid
				// and we want each asteroid to rotate around its center
				GL.Translate(boundingCircle.CenterX, boundingCircle.CenterY, 0f);
				GL.Scale(2f * boundingCircle.Radius, 2f * boundingCircle.Radius, 1f);
				GL.Rotate(time.AbsoluteTime * 70, 0, 0, 1);
				GL.Begin(PrimitiveType.Polygon);
				foreach (var p in asteroid)
				{
					GL.Vertex2(p);
				}
				GL.End();
			}
			GL.PopMatrix();
		}
	}
}
