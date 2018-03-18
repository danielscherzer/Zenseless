using System;
using System.Collections.Generic;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	internal class Model
	{
		private Box2D movingObject = new Box2D(0, 0, .2f, .2f);
		private List<Vector2> wayPoints = new List<Vector2>();
		private List<Vector2> wayTangents;

		public Model()
		{
			//set way-points of enemy
			wayPoints.Add(new Vector2(-.5f, -.5f));
			wayPoints.Add(new Vector2(.5f, -.5f));
			wayPoints.Add(new Vector2(.5f, .5f));
			wayPoints.Add(new Vector2(-.5f, .5f));
			//wayPoints.Add(new Vector2(.6f, -.7f));
			//wayPoints.Add(new Vector2(.5f, .8f));
			//wayPoints.Add(new Vector2(-.5f, .4f));
			//wayPoints.Add(new Vector2(0, 0));
			wayTangents = CatmullRomSpline.FiniteDifferenceLoop(wayPoints);
		}

		public IReadOnlyBox2D MovingObject => movingObject;

		public void Update(float interpolationParameter)
		{
			var activeSegment = CatmullRomSpline.FindSegmentLoop(interpolationParameter, wayPoints.Count);
			var pos = CatmullRomSpline.EvaluateSegment(wayPoints[activeSegment.Item1]
				, wayPoints[activeSegment.Item2]
				, wayTangents[activeSegment.Item1]
				, wayTangents[activeSegment.Item2]
				, interpolationParameter - (float)Math.Floor(interpolationParameter));

			movingObject.MinX = pos.X;
			movingObject.MinY = pos.Y;
		}
	}
}