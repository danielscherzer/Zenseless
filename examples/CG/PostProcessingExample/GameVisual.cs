namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System.Drawing;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class GameVisual
	{
		public GameVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			texBackground = contentLoader.Load<ITexture2D>("background");
			texBird = contentLoader.Load<ITexture2D>("bird");

			//for transparency in textures we use blending
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
		}

		public void Render()
		{
			GL.Color3(Color.White);
			//draw background
			texBackground.Activate();
			background.DrawTexturedRect(Box2D.BOX01);
			texBackground.Deactivate();

			//draw player
			texBird.Activate();
			bird.DrawTexturedRect(Box2D.BOX01);
			texBird.Deactivate();
		}

		public void Update(float updatePeriod)
		{
			var t = Transformation.Rotation(-200f * updatePeriod);
			bird.TransformCenter(t);
		}

		private IReadOnlyBox2D background = new Box2D(-1.0f, -1.0f, 2.0f, 2.0f);
		private Box2D bird = Box2DExtensions.CreateFromCenterSize(0.0f, -0.8f, 0.3f, 0.3f);
		private ITexture texBird;
		private ITexture texBackground;
	}
}
