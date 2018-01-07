using Zenseless.Geometry;
using Zenseless.HLGL;
using System;
using System.Collections.Generic;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IAnimation" />
	public class AnimationTextures : IAnimation
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AnimationTextures"/> class.
		/// </summary>
		/// <param name="animationLength">Length of the animation.</param>
		public AnimationTextures(float animationLength)
		{
			this.AnimationLength = animationLength;
		}

		/// <summary>
		/// Adds the frame.
		/// </summary>
		/// <param name="textureFrame">The texture frame.</param>
		public void AddFrame(ITexture textureFrame)
		{
			textures.Add(textureFrame);
		}

		/// <summary>
		/// Gets or sets the length of the animation.
		/// </summary>
		/// <value>
		/// The length of the animation.
		/// </value>
		public float AnimationLength { get; set; }

		/// <summary>
		/// Gets the textures.
		/// </summary>
		/// <value>
		/// The textures.
		/// </value>
		public IList<ITexture> Textures
		{
			get
			{
				return textures;
			}
		}

		/// <summary>
		/// Calculates the frame id (the current frame of the animation) out of the given time
		/// </summary>
		/// <param name="time"></param>
		/// <returns>id of the current frame of the animation</returns>
		public uint CalcAnimationFrame(float time)
		{
			float normalizedDeltaTime = (time % AnimationLength) / AnimationLength;
			double idF = normalizedDeltaTime * (textures.Count - 1);
			uint id = (uint) Math.Max(0, Math.Round(idF));
			return id;
		}

		/// <summary>
		/// draws a GL quad, textured with an animation.
		/// </summary>
		/// <param name="rectangle">coordinates of the GL quad</param>
		/// <param name="totalSeconds">animation position in seconds</param>
		public void Draw(IReadOnlyBox2D rectangle, float totalSeconds)
		{
			var id = (int)CalcAnimationFrame(totalSeconds);
			textures[id].Activate();
			rectangle.DrawTexturedRect(Box2D.BOX01);
			textures[id].Deactivate();
		}

		private List<ITexture> textures = new List<ITexture>();
	}
}
