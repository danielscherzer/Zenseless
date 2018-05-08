using Zenseless.ExampleFramework;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.HLGL;

namespace Example
{
	class MyVisual
	{
		private TextureFont font;

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual(window.RenderContext.RenderState, window.ContentLoader);
			window.Render += visual.Render;
			window.Run();

		}

		private MyVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			//for transparency in textures we use blending
			renderState.Set(BlendStates.AlphaBlend);

			//load font
			font = new TextureFont(contentLoader.Load<ITexture2D>("Blood Bath 2"), 10, 32, .8f, 1, .7f);
			//background clear color
			renderState.Set(new ClearColorState(0, 0, 0, 1));

			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point
			GL.Color3(Color.White); //color is multiplied with texture color white == no change
		}

		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			font.Print(-.9f, -.125f, 0, .25f, "SUPER GEIL"); //print string
		}
	}
}