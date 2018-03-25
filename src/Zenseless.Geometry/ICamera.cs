using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Interface to a camera
	/// </summary>
	public interface ICamera
	{
		/// <summary>
		/// Gets or sets the aspect ratio.
		/// </summary>
		/// <value>
		/// The aspect ratio.
		/// </value>
		float Aspect { get; }

		/// <summary>
		/// Gets or sets the far clipping plane distance.
		/// </summary>
		/// <value>
		/// The far clipping plane distance.
		/// </value>
		float FarClip { get; }

		/// <summary>
		/// Gets or sets the field-of-view y.
		/// </summary>
		/// <value>
		/// The field-of-view y.
		/// </value>
		float FovY { get; }

		/// <summary>
		/// Gets or sets the near clipping plane distance.
		/// </summary>
		/// <value>
		/// The near clipping plane distance.
		/// </value>
		float NearClip { get; }

		/// <summary>
		/// Calculates the complete transformation matrix (projection and view).
		/// </summary>
		/// <returns></returns>
		Matrix4x4 CalcMatrix();

		/// <summary>
		/// Calculates the projection matrix.
		/// </summary>
		/// <returns></returns>
		Matrix4x4 CalcProjectionMatrix();

		/// <summary>
		/// Calculates the view matrix.
		/// </summary>
		/// <returns></returns>
		Matrix4x4 CalcViewMatrix();
	}
}