using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using Zenseless.ExampleFramework;
using Zenseless.Base;
using Zenseless.Geometry;

namespace Example
{
	class Controller
	{
		private List<Collider> colliders = new List<Collider>();
		private IReadOnlyBox2D windowBorders = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private CollisionGrid collisionGrid;
		private GameTime time;
		private double lastBenchmark = 0;

		private Controller()
		{
			SetupColliders();
			time = new GameTime();
		}

		private void SetupColliders()
		{
			float delta = 0.03f;
			float space = 0.01f;
			float distance2 = space / 2;
			float size = delta - space;
			int i = 0;
			for (float x =  -0.9f; x < 0.9f; x += delta)
			{
				for (float y = -0.9f; y < 0.9f; y += delta)
				{
					var collider = new Collider(x, y, size, size)
					{
						Velocity = RandomVectors.Velocity()
					};
					colliders.Add(collider);
					++i;
				}
			}
			float scale = 2f;
			collisionGrid = new CollisionGrid(windowBorders, size * scale, size * scale);
		}

		private void Update(float updatePeriod, Action<double> actionBenchmark)
		{
			//movement
			foreach (var collider in colliders)
			{
				collider.SaveBox();
				collider.Box.MinX += collider.Velocity.X * updatePeriod;
				collider.Box.MinY += collider.Velocity.Y * updatePeriod;
				if (!windowBorders.Contains(collider.Box))
				{
					collider.Box.PushXRangeInside(windowBorders);
					collider.Box.PushYRangeInside(windowBorders);
					collider.Velocity = -collider.Velocity;
				}
			}

			var t1 = time.AbsoluteMilliseconds; //get time before collision detection
			//handle collisions
			if(Keyboard.GetState().IsKeyDown(Key.Space))
			{
				BruteForceCollision();
			}
			else
			{
				GridCollision();
			}
			var t2 = time.AbsoluteMilliseconds; //get time after collision detection

			if (t2 > lastBenchmark + 500.0)
			{
				actionBenchmark?.Invoke(t2 - t1);
				lastBenchmark = t2;
			}
		}

		private void BruteForceCollision()
		{
			for (int i = 0; i < colliders.Count; ++i)
			{
				for (int j = i + 1; j < colliders.Count; ++j)
				{
					 HandleNarrowPhaseCollision(colliders[i], colliders[j]);
				}
			}
		}

		private void GridCollision()
		{
			collisionGrid.FindAllCollisions(colliders, (c1, c2) => HandleNarrowPhaseCollision(c1 as Collider, c2 as Collider));
		}

		private void HandleNarrowPhaseCollision(Collider collider1, Collider collider2)
		{
			var box1 = collider1.Box;
			var box2 = collider2.Box;
			if (box1.Intersects(box2))
			{
				//undo movement
				collider1.RestoreSavedBox();
				collider2.RestoreSavedBox();
				////set random velocity
				collider1.Velocity = RandomVectors.Velocity();
				collider2.Velocity = RandomVectors.Velocity();
			}
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			foreach (var collider in colliders)
			{
				GL.Color3(collider.Color);
				DrawBox(collider.Box);
			}
		}
		
		private static void DrawBox(IReadOnlyBox2D rect)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(rect.MinX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MinY);
			GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow(512, 512, 30);
			var controller = new Controller();
			window.Update += (t) => controller.Update(t, (timing) => window.GameWindow.Title = $"{Math.Round(timing)}ms");
			window.Render += controller.Render;
			window.Run();
			window.Dispose();
		}
	}
}