using Zenseless.Geometry;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAnimation
	{
		/// <summary>
		/// Gets or sets the length of the animation.
		/// </summary>
		/// <value>
		/// The length of the animation.
		/// </value>
		float AnimationLength { get; set; }

		/// <summary>
		/// Draws the specified rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="totalSeconds">The total seconds.</param>
		void Draw(IReadOnlyBox2D rectangle, float totalSeconds);
	}
}