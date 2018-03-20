using Zenseless.ExampleFramework;
using Zenseless.Geometry;
using Zenseless.OpenGL;

namespace Zenseless.ExampleFramework
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExampleWindowExtensions
	{
		/// <summary>
		/// Add Maya like camera handling. 
		/// </summary>
		/// <param name="window">window that receives input system events</param>
		/// <param name="camera">orbit camera events should be routed too.</param>
		public static void AddMayaCameraEvents(this ExampleWindow window, CameraOrbit camera)
		{
			window.GameWindow.AddMayaCameraEvents(camera);
		}
	}
}