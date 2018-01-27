using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Zenseless.ContentPipeline
{
	/// <summary>
	/// 
	/// </summary>
	public static class FormTools
	{
		/// <summary>
		/// Determines whether [is partly on screen] [the specified bounds].
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		/// <returns>
		///   <c>true</c> if [is partly on screen] [the specified bounds]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsPartlyOnScreen(Rectangle bounds)
		{
			return Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(bounds));
		}
		/// <summary>
		/// Determines whether [is point on screen] [the specified point].
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>
		///   <c>true</c> if [is point on screen] [the specified point]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsPointOnScreen(Point point)
		{
			return Screen.AllScreens.Any(s => s.WorkingArea.Contains(point));
		}
	}
}
