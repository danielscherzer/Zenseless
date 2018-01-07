using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public static class CircleExtensions
	{
		/// <summary>
		/// Creates from box.
		/// </summary>
		/// <param name="box">The box.</param>
		/// <returns></returns>
		public static Circle CreateFromBox(IReadOnlyBox2D box)
		{
			var circle = new Circle(box.CenterX, box.CenterY, 0.5f * Math.Min(box.SizeX, box.SizeY));
			return circle;
		}

		/// <summary>
		/// Creates from minimum maximum.
		/// </summary>
		/// <param name="minX">The minimum x.</param>
		/// <param name="minY">The minimum y.</param>
		/// <param name="maxX">The maximum x.</param>
		/// <param name="maxY">The maximum y.</param>
		/// <returns></returns>
		public static Circle CreateFromMinMax(float minX, float minY, float maxX, float maxY)
		{
			var box = Box2DExtensions.CreateFromMinMax(minX, minY, maxX, maxY);
			return CreateFromBox(box);
		}

		/// <summary>
		/// Pushes the x range inside.
		/// </summary>
		/// <param name="circle">The circle.</param>
		/// <param name="minX">The minimum x.</param>
		/// <param name="maxX">The maximum x.</param>
		/// <returns></returns>
		public static bool PushXRangeInside(this Circle circle, float minX, float maxX)
		{
			if (circle.Radius > maxX - minX) return false;
			if (circle.CenterX - circle.Radius < minX)
			{
				circle.CenterX = minX + circle.Radius;
			}
			if (circle.CenterX + circle.Radius > maxX)
			{
				circle.CenterX = maxX - circle.Radius;
			}
			return true;
		}

		/// <summary>
		/// Pushes the y range inside.
		/// </summary>
		/// <param name="circle">The circle.</param>
		/// <param name="minY">The minimum y.</param>
		/// <param name="maxY">The maximum y.</param>
		/// <returns></returns>
		public static bool PushYRangeInside(this Circle circle, float minY, float maxY)
		{
			if (circle.Radius > maxY - minY) return false;
			if (circle.CenterY - circle.Radius < minY)
			{
				circle.CenterY = minY + circle.Radius;
			}
			if (circle.CenterY + circle.Radius > maxY)
			{
				circle.CenterY = maxY - circle.Radius;
			}
			return true;
		}

		/// <summary>
		/// Undoes the overlap.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		public static void UndoOverlap(this Circle a, Circle b)
		{
			Vector2 cB = new Vector2(b.CenterX, b.CenterY);
			Vector2 diff = new Vector2(a.CenterX, a.CenterY);
			diff -= cB;
			diff /= diff.Length();
			diff *= a.Radius + b.Radius;
			var newA = cB + diff;
			a.CenterX = newA.X;
			a.CenterY = newA.Y;
		}
	}
}
