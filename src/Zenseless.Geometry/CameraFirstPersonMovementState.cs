using System.Numerics;

namespace Zenseless.Geometry
{
	public class CameraFirstPersonMovementState
	{
		public float X { get; set; } = 0f;
		public float Y { get; set; } = 0f;
		public float Z { get; set; } = 0f;

		public void Update(CameraFirstPerson camera, float speed)
		{
			var rotation = camera.CalcRotationMatrix();
			var movement = new Vector3(X, Y, Z) * speed;
			camera.Position += Vector3.Transform(movement, rotation);
		}
	}
}
