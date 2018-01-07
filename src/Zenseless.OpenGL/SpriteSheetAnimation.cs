using Zenseless.Geometry;
using System;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IAnimation" />
	public class SpriteSheetAnimation : IAnimation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpriteSheetAnimation"/> class.
		/// </summary>
		/// <param name="spriteSheet">The sprite sheet.</param>
		/// <param name="fromID">From identifier.</param>
		/// <param name="toID">To identifier.</param>
		/// <param name="animationLength">Length of the animation.</param>
		public SpriteSheetAnimation(SpriteSheet spriteSheet, uint fromID, uint toID, float animationLength)
		{
			this.SpriteSheet = spriteSheet;
			FromID = fromID;
			ToID = toID;
			AnimationLength = animationLength;
		}

		/// <summary>
		/// Gets or sets the length of the animation.
		/// </summary>
		/// <value>
		/// The length of the animation.
		/// </value>
		public float AnimationLength { get; set; }
		/// <summary>
		/// Gets or sets from identifier.
		/// </summary>
		/// <value>
		/// From identifier.
		/// </value>
		public uint FromID { get; set; }
		/// <summary>
		/// Gets the sprite sheet.
		/// </summary>
		/// <value>
		/// The sprite sheet.
		/// </value>
		public SpriteSheet SpriteSheet { get; private set; }
		/// <summary>
		/// Gets or sets to identifier.
		/// </summary>
		/// <value>
		/// To identifier.
		/// </value>
		public uint ToID { get; set; }

		/// <summary>
		/// Calculates the sprite id (the current frame of the animation) out of the given time
		/// </summary>
		/// <param name="fromID">sprite id for first animation frame</param>
		/// <param name="toID">sprite id for last animation frame</param>
		/// <param name="animationLength">total animation time in seconds</param>
		/// <param name="time">current time</param>
		/// <returns>
		/// sprite id of the current frame of the animation
		/// </returns>
		public static uint CalcAnimationSpriteID(uint fromID, uint toID, float animationLength, float time)
		{
			float normalizedDeltaTime = (time % animationLength) / animationLength;
			float id = fromID + normalizedDeltaTime * (toID - fromID);
			return (uint)Math.Round(id);
		}

		/// <summary>
		/// draws a GL quad, textured with an animation.
		/// </summary>
		/// <param name="rectangle">coordinates ofthe GL quad</param>
		/// <param name="totalSeconds">animation position in seconds</param>
		public void Draw(IReadOnlyBox2D rectangle, float totalSeconds)
		{
			var id = CalcAnimationSpriteID(FromID, ToID, AnimationLength, totalSeconds);
			var texCoords = SpriteSheet.CalcSpriteTexCoords(id);
			SpriteSheet.Activate();
			rectangle.DrawTexturedRect(texCoords);
			SpriteSheet.Deactivate();
		}
	}
}
