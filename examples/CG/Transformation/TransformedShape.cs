using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	/// <summary>
	/// Contains transformed and internally untransformed version of Box2D
	/// </summary>
	public class TransformedShape
	{
		public TransformedShape(Matrix3x2 transformation, Box2D bounds)
		{
			this.bounds = bounds;
			TransformedBounds = new Box2D(bounds);
			Transformation = transformation;
		}

		/// <summary>
		/// Gets or sets the transformation that will be applied to bounds to create TransformedBounds.
		/// </summary>
		/// <value>
		/// The transformation.
		/// </value>
		public Matrix3x2 Transformation
		{
			get => transformation;
			set
			{
				transformation = value;
				var newCenter = Vector2.Transform(bounds.GetCenter(), Transformation);
				TransformedBounds.CenterX = newCenter.X;
				TransformedBounds.CenterY = newCenter.Y;

				//corner points of box
				//var points = bounds.CalcCornerPoints();
				//var transformedPoints = new Vector2[4];
				//for(int i = 0; i < 4; ++i)
				//{
				//	transformedPoints[i] = Vector2.Transform(points[i], Transformation);
				//}
				//TransformedBounds = transformedPoints.CreateFromPoints();
			}
		}
		public Box2D TransformedBounds { get; private set; }

		private readonly Box2D bounds;
		private Matrix3x2 transformation;
	}
}
