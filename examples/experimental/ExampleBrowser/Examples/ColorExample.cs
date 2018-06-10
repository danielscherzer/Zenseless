namespace ExampleBrowser
{
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.ComponentModel.Composition;
	using System.Numerics;
	using Zenseless.Patterns;
	using Zenseless.Geometry;
	using Zenseless.OpenGL;

	[ExampleDisplayName]
	[Export(typeof(IExample))]
	public class ColorExample : Example
	{
		private readonly ITime time;
		public float Hue { get; private set; } = 0f;

		[ImportingConstructor]
		public ColorExample([Import] ITime time)
		{
			this.time = time;
		}

		public override void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			const int count = 16;
			var boxSize = 1.9f / count;
			for (int xI = 0; xI < count; ++xI)
			{
				float x = xI * 2f / count - 1f;
				float brightness = xI / (float)count;
				for (int yI = 0; yI < count; ++yI)
				{
					float y = yI * 2f / count - 1f;
					float saturation = yI / (float)count;
					var color = ColorSystems.Hsb2rgb(Hue, saturation, brightness);
					DrawRect(new Box2D(x, y, boxSize, boxSize), color);
				}
			}
		}

		public override void Update()
		{
			Hue = (float)(Math.Sin(time.AbsoluteTime) * 0.5 + 0.5);
			RaisePropertyChanged(nameof(Hue));
		}

		private void DrawRect(IReadOnlyBox2D rectangle, Vector3 color)
		{
			GL.Color3(color.X, color.Y, color.Z);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(rectangle.MinX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MinY);
			GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.Vertex2(rectangle.MinX, rectangle.MaxY);
			GL.End();
		}
	}
}