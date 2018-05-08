namespace Example
{
	using System;
	using System.Drawing;
	using System.IO;
	using Zenseless.Base;
	using Zenseless.ExampleFramework;
	using Zenseless.OpenGL;

	public class MyApplication
	{
		[STAThread]
		public static void Main()
		{
			var window = new ExampleWindow();
			var canvas = new Canvas(window.RenderContext.RenderState);
			Bitmap screenshot = null; 
			var rasterizer = new Rasterizer(window.ContentLoader, 10, 10, canvas.Draw);
			window.Render += rasterizer.Render;
			window.Render += () => screenshot = FrameBuffer.ToBitmap();
			window.Run();

			if (screenshot is null) return;
			var name = Path.ChangeExtension(PathTools.GetCurrentProcessPath(), ".png");
			screenshot.Save(name);
			screenshot.SaveToClipboard();
		}
	}
}