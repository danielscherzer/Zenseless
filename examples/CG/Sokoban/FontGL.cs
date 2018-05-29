using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL;
using Zenseless.HLGL;

namespace Example
{
	internal class FontGL
	{
		public FontGL(IContentLoader contentLoader)
		{
			//shdFont = context.CreateShader();
			//shdFont.FromStrings(Tools.ToString(Resourcen.texColorVert), Tools.ToString(Resourcen.texColorFrag));
			//locCamera = shdFont.GetResourceLocation(ShaderResourceType.Uniform, "camera");
			GL.Enable(EnableCap.Texture2D); //TODO: only for non shader pipeline relevant -> remove at some point

			font = new TextureFont(contentLoader.Load<ITexture2D>("Video Phreak"), 10, 32, 1, 1, .7f);
		}

		public void Print(string message, float size, TextAlignment alignment = TextAlignment.Left)
		{
			//var camera = Matrix4x4.CreateOrthographicOffCenter(0, windowAspect, 0, 1, 0, 1).ToOpenTK();
			//GL.ProgramUniformMatrix4(ProgramID, locCamera, false, ref camera);

			GL.LoadIdentity();
			GL.Ortho(0, windowAspect, 0, 1, 0, 1);
			var alignmentDelta = 0f;
			switch (alignment)
			{
				case TextAlignment.Right: alignmentDelta = windowAspect - font.Width(message, size); break;
				case TextAlignment.Center: alignmentDelta = .5f * (windowAspect - font.Width(message, size)); break;
			}
			font.Print(alignmentDelta, 0, 0, size, message);
		}

		internal void ResizeWindow(int width, int height)
		{
			windowAspect = width / (float)height;
		}

		//private IShader shdFont;
		private TextureFont font;
		private float windowAspect;
		//private int locCamera;
	}
}