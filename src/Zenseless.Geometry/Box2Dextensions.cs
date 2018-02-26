using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// This class contains static extension methods for Box2D.
	/// </summary>
	public static class Box2DExtensions
	{
		/// <summary>
		/// Create a Box2D from min and max coordinates (calculates the size on creation)
		/// </summary>
		/// <param name="minX">Minimal X</param>
		/// <param name="minY">Minimal Y</param>
		/// <param name="maxX">Maximal X</param>
		/// <param name="maxY">Maximal Y</param>
		/// <returns>A new Box2D instance</returns>
		public static Box2D CreateFromMinMax(float minX, float minY, float maxX, float maxY)
		{
			var rectangle = new Box2D(minX, minY, maxX - minX, maxY - minY);
			return rectangle;
		}

		/// <summary>
		/// Create a Box2D from min and max coordinates (calculates the size on creation)
		/// </summary>
		/// <param name="min">Minimal point</param>
		/// <param name="max">Maximal point</param>
		/// <returns>A new Box2D instance</returns>
		public static Box2D CreateFromMinMax(Vector2 min, Vector2 max)
		{
			var rectangle = new Box2D(min.X, min.Y, max.X - min.X, max.Y - min.Y);
			return rectangle;
		}
		/// <summary>
		/// Creates a box from corner points.
		/// </summary>
		/// <param name="a">An input corner point.</param>
		/// <param name="b">An input corner point.</param>
		/// <returns>A box containing the points.</returns>
		public static Box2D CreateFromPoints(Vector2 a, Vector2 b)
		{
			var minX = Math.Min(a.X, b.X);
			var minY = Math.Min(a.Y, b.Y);
			var maxX = Math.Max(a.X, b.X);
			var maxY = Math.Max(a.Y, b.Y);
			return CreateFromMinMax(minX, minY, maxX, maxY);
		}

		/// <summary>
		/// Create a <seealso cref="Box2D"/> from its center and size (calculates the min coordinates on creation)
		/// </summary>
		/// <param name="centerX">Center x</param>
		/// <param name="centerY">Center y</param>
		/// <param name="sizeX">Size x</param>
		/// <param name="sizeY">Size y</param>
		/// <returns>A new Box2D instance</returns>
		public static Box2D CreateFromCenterSize(float centerX, float centerY, float sizeX, float sizeY)
		{
			var rectangle = new Box2D(0, 0, sizeX, sizeY)
			{
				CenterX = centerX,
				CenterY = centerY
			};
			return rectangle;
		}

		/// <summary>
		/// Creates a <seealso cref="Box2D"/> from a circle.
		/// </summary>
		/// <param name="circle">The circle.</param>
		/// <returns>A new Box2D instance</returns>
		public static Box2D CreateFromCircle(IReadOnlyCircle circle)
		{
			var rectangle = new Box2D(0, 0, 2f * circle.Radius, 2f * circle.Radius)
			{
				CenterX = circle.CenterX,
				CenterY = circle.CenterY
			};
			return rectangle;
		}
		
		/// <summary>
				/// Checks if point is inside the rectangle (including borders)
				/// </summary>
				/// <param name="rectangle">Rectangle to check</param>
				/// <param name="point">Coordinates of the point</param>
				/// <returns>true if point is inside the rectangle (including borders)</returns>
		public static bool Contains(this IReadOnlyBox2D rectangle, Vector2 point) => rectangle.Contains(point.X, point.Y);

		/// <summary>
		/// Gets the center of the box.
		/// </summary>
		/// <param name="box">The box.</param>
		/// <returns></returns>
		public static Vector2 GetCenter(this IReadOnlyBox2D box)
		{
			return new Vector2(box.CenterX, box.CenterY);
		}

		/// <summary>
		/// Merges the specified boxes.
		/// </summary>
		/// <param name="boxA">The box a.</param>
		/// <param name="boxB">The box b.</param>
		/// <returns>A box that contains boxA and boxB.</returns>
		/// <exception cref="ArgumentNullException">
		/// boxA
		/// or
		/// boxB
		/// </exception>
		public static Box2D Merge(this Box2D boxA, Box2D boxB)
		{
			if (boxA == null) throw new ArgumentNullException(nameof(boxA));
			if (boxB == null) throw new ArgumentNullException(nameof(boxB));
			var minX = Math.Min(boxA.MinX, boxB.MinX);
			var minY = Math.Min(boxA.MinY, boxB.MinY);
			var maxX = Math.Max(boxA.MaxX, boxB.MaxX);
			var maxY = Math.Max(boxA.MaxY, boxB.MaxY);
			return CreateFromMinMax(minX, minY, maxX, maxY);
		}

		/// <summary>
		/// Pushes rectangleA inside rectangleB, but only in regards to the x-direction
		/// </summary>
		/// <param name="rectangleA">rectangle to push</param>
		/// <param name="rectangleB">bounds to push inside of</param>
		/// <returns>true if a push was necessary</returns>
		public static bool PushXRangeInside(this Box2D rectangleA, IReadOnlyBox2D rectangleB)
		{
			if (rectangleA.SizeX > rectangleB.SizeX) return false;
			if (rectangleA.MinX < rectangleB.MinX)
			{
				rectangleA.MinX = rectangleB.MinX;
			}
			if (rectangleA.MaxX > rectangleB.MaxX)
			{
				rectangleA.MinX = rectangleB.MaxX - rectangleA.SizeX;
			}
			return true;
		}

		/// <summary>
		/// Pushes rectangleA inside rectangleB, but only in regards to the y-direction
		/// </summary>
		/// <param name="rectangleA">rectangle to push</param>
		/// <param name="rectangleB">bounds to push inside of</param>
		/// <returns>true if a push was necessary</returns>
		public static bool PushYRangeInside(this Box2D rectangleA, IReadOnlyBox2D rectangleB)
		{
			if (rectangleA.SizeY > rectangleB.SizeY) return false;
			if (rectangleA.MinY < rectangleB.MinY)
			{
				rectangleA.MinY = rectangleB.MinY;
			}
			if (rectangleA.MaxY > rectangleB.MaxY)
			{
				rectangleA.MinY = rectangleB.MaxY - rectangleA.SizeY;
			}
			return true;
		}

		/// <summary>
		/// Calculates the overlap Box
		/// Returns null if no overlap
		/// </summary>
		/// <param name="rectangleA"></param>
		/// <param name="rectangleB"></param>
		/// <returns>AABR in the overlap</returns>
		public static Box2D Overlap(this IReadOnlyBox2D rectangleA, IReadOnlyBox2D rectangleB)
		{
			Box2D overlap = null;

			if (rectangleA.Intersects(rectangleB))
			{
				overlap = new Box2D(0.0f, 0.0f, 0.0f, 0.0f)
				{
					MinX = (rectangleA.MinX < rectangleB.MinX) ? rectangleB.MinX : rectangleA.MinX,
					MinY = (rectangleA.MinY < rectangleB.MinY) ? rectangleB.MinY : rectangleA.MinY
				};
				overlap.SizeX = (rectangleA.MaxX < rectangleB.MaxX) ? rectangleA.MaxX - overlap.MinX : rectangleB.MaxX - overlap.MinX;
				overlap.SizeY = (rectangleA.MaxY < rectangleB.MaxY) ? rectangleA.MaxY - overlap.MinY : rectangleB.MaxY - overlap.MinY;
			}

			return overlap;
		}

		/// <summary>
		/// Transforms the center of a rectangle by a matrix
		/// </summary>
		/// <param name="rectangle">to transform</param>
		/// <param name="M">transformation matrix to apply</param>
		public static void TransformCenter(this Box2D rectangle, Matrix3x2 M)
		{
			Vector2 center = new Vector2(rectangle.CenterX, rectangle.CenterY);
			var newCenter = Vector2.Transform(center, M);
			rectangle.CenterX = newCenter.X;
			rectangle.CenterY = newCenter.Y;
		}

		/// <summary>
		/// If an intersection with the frame occurs do the minimal translation to undo the overlap
		/// </summary>
		/// <param name="rectangleA">The rectangle that will be moved to avoid intersection</param>
		/// <param name="rectangleB">The rectangle to check for intersection</param>
		public static void UndoOverlap(this Box2D rectangleA, IReadOnlyBox2D rectangleB)
		{
			if (!rectangleA.Intersects(rectangleB)) return;

			Vector2[] directions = new Vector2[]
			{
				new Vector2(rectangleB.MaxX - rectangleA.MinX, 0), // push distance A in positive X-direction
				new Vector2(rectangleB.MinX - rectangleA.MaxX, 0), // push distance A in negative X-direction
				new Vector2(0, rectangleB.MaxY - rectangleA.MinY), // push distance A in positive Y-direction
				new Vector2(0, rectangleB.MinY - rectangleA.MaxY)  // push distance A in negative Y-direction
			};
			float[] pushDistSqrd = new float[4];
			for (int i = 0; i < 4; ++i)
			{
				pushDistSqrd[i] = directions[i].LengthSquared();
			}
			//find minimal positive overlap amount
			int minId = 0;
			for (int i = 1; i < 4; ++i)
			{
				minId = pushDistSqrd[i] < pushDistSqrd[minId] ? i : minId;
			}

			rectangleA.MinX += directions[minId].X;
			rectangleA.MinY += directions[minId].Y;
		}

		/// <summary>
		/// Create a box that is at least size with x height, but has aspect ratio newWidth2heigth
		/// </summary>
		/// <param name="width">minimal width</param>
		/// <param name="height">minimal height</param>
		/// <param name="newWidth2heigth">new aspect ratio</param>
		/// <returns>A box that is at least size with x height, but has aspect ratio newWidth2heigth</returns>
		public static Box2D CreateContainingBox(float width, float height, float newWidth2heigth)
		{
			float fWinAspect = width / height;
			bool isLandscape = newWidth2heigth < fWinAspect;
			float outputWidth = isLandscape ? width : height * newWidth2heigth;
			float outputHeight = isLandscape ? width / newWidth2heigth : height;
			var x = isLandscape ? 0f : (width - outputWidth) * .5f;
			var y = isLandscape ? (height - outputHeight) * .5f : 0f;
			return new Box2D(x, y, outputWidth, outputHeight);
		}
	}
}
