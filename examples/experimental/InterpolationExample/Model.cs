using System.Collections.Generic;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	internal class Model
	{
		private readonly Box2D movingObject = new Box2D(0, 0, .2f, .2f);
		private readonly List<Vector2> wayPoints = new List<Vector2>();

		public Model()
		{
			//set way-points of bird
			wayPoints.Add(new Vector2(-.5f, -.5f));
			wayPoints.Add(new Vector2(.5f, -.5f));
			wayPoints.Add(new Vector2(.5f, .5f));
			wayPoints.Add(new Vector2(-.5f, .5f));
			//wayPoints.Add(new Vector2(-.5f, -.5f));
			//wayPoints.Add(new Vector2(.6f, -.7f));
			//wayPoints.Add(new Vector2(.5f, .8f));
			//wayPoints.Add(new Vector2(-.5f, .4f));
			//wayPoints.Add(new Vector2(0, 0));
		}

		public IReadOnlyBox2D MovingObject => movingObject;

		public void Update(float interpolationParameter)
		{
			var pos = CubicHermiteSpline.CatmullRomSplineLoop(wayPoints, interpolationParameter);
			movingObject.MinX = pos.X;
			movingObject.MinY = pos.Y;
		}
	}
}