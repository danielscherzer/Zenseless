using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.Base;
using Zenseless.ExampleFramework;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	class MyVisual
	{
		private SpriteSheetAnimation explosion;
		private SpriteSheetAnimation girlIdleRun;
		private SpriteSheetAnimation girlJumpBall;
		private SpriteSheetAnimation girlFight;
		private SpriteSheetAnimation girlDie;
		private SpriteSheetAnimation girlBack;
		private AnimationTextures alienShip;

		private MyVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			//animation using a single SpriteSheet
			explosion = new SpriteSheetAnimation(new SpriteSheet(contentLoader.Load<ITexture2D>("explosion"), 5, 5), 0, 24, 1f);

			//art from https://github.com/sparklinlabs/superpowers-asset-packs
			var spriteSheetGirl = new SpriteSheet(contentLoader.Load<ITexture2D>("girl-2"), 6, 7);
			girlIdleRun = new SpriteSheetAnimation(spriteSheetGirl, 0, 10, 1f);
			girlJumpBall = new SpriteSheetAnimation(spriteSheetGirl, 11, 20, 1f);
			girlFight = new SpriteSheetAnimation(spriteSheetGirl, 21, 25, 1f);
			girlDie = new SpriteSheetAnimation(spriteSheetGirl, 25, 32, 1f);
			girlBack = new SpriteSheetAnimation(spriteSheetGirl, 33, 36, 1f);

			//animation using a bitmap for each frame
			alienShip = new AnimationTextures(.5f);
			//art from http://millionthvector.blogspot.de/p/free-sprites.html
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10001"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10002"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10003"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10004"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10005"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10006"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10007"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10008"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10009"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10010"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10011"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10012"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10013"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10014"));
			alienShip.AddFrame(contentLoader.Load<ITexture2D>("alien10015"));

			//for transparency in textures
			renderState.Set(BlendStates.AlphaBlend);
			GL.Enable(EnableCap.Texture2D); //TODO: remove if shader is used
		}

		private void Render(float absoluteTimeSeconds)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);

			explosion.Draw(new Box2D(-.7f, .2f, .4f, .4f), absoluteTimeSeconds);
			alienShip.Draw(new Box2D(.3f, .2f, .4f, .4f), absoluteTimeSeconds);

			girlIdleRun.Draw(new Box2D(-1f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlJumpBall.Draw(new Box2D(-.6f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlFight.Draw(new Box2D( -.2f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlDie.Draw(new Box2D(.2f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlBack.Draw(new Box2D(.6f, -.6f, .4f, .4f), absoluteTimeSeconds);
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual(window.RenderContext.RenderState, window.ContentLoader);
			var time = new GameTime();
			window.Render += () => visual.Render(time.AbsoluteTime);
			window.Run();
			window.Dispose();
		}
	}
}