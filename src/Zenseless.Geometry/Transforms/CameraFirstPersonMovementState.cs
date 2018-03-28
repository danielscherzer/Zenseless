namespace Zenseless.Geometry
{
	using System.Numerics;

	/// <summary>
	/// 
	/// </summary>
	public class CameraFirstPersonMovementState
	{
		/// <summary>
		/// Gets or sets the x.
		/// </summary>
		/// <value>
		/// The x.
		/// </value>
		public float X { get; set; } = 0f;
		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		/// <value>
		/// The y.
		/// </value>
		public float Y { get; set; } = 0f;
		/// <summary>
		/// Gets or sets the z.
		/// </summary>
		/// <value>
		/// The z.
		/// </value>
		public float Z { get; set; } = 0f;

		/// <summary>
		/// Updates the specified camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		/// <param name="speed">The speed.</param>
		public void Update(CameraFirstPerson camera, float speed)
		{
			var rotation = camera.CalcRotationMatrix();
			var movement = new Vector3(X, Y, Z) * speed;
			camera.Position += Vector3.Transform(movement, rotation);
		}
	}
}
