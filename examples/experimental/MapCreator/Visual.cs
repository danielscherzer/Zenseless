namespace Example
{
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class Visual
	{
		private readonly IRenderSurface fbo;
		private readonly PostProcessing copyToFrameBuffer;
		
		public Visual(IRenderState renderState, IContentLoader contentLoader)
		{
			fbo = new FBO(Texture2dGL.Create(70, 70));
			fbo.Texture.Filter = TextureFilterMode.Nearest;
			copyToFrameBuffer = new PostProcessing(contentLoader.LoadPixelShader("copy.frag"));

			renderState.Set(BlendStates.AlphaBlend);
			renderState.Set(new LineSmoothing(true));
			renderState.Set(new LineWidth(5f));
			GL.Enable(EnableCap.PointSmooth);
		}

		internal void Render(IEnumerable<IEnumerable<Vector2>> paths, IEnumerable<Vector2> points)
		{
			var random = new Random(12);
			fbo.Draw(() =>
			{
				GL.Clear(ClearBufferMask.ColorBufferBit);
				foreach (var path in paths)
				{
					byte[] color = new byte[2];
					random.NextBytes(color);
					GL.Color3(color[0], color[1], (byte)255);
					DrawLine(path);
				}

				GL.Color3(1f, 0f, 0f);
				GL.PointSize(30f);
				DrawPoints(points);

			});
			GL.Clear(ClearBufferMask.ColorBufferBit);
			copyToFrameBuffer.Draw(fbo.Texture);
		}

		private void DrawPoints(IEnumerable<Vector2> points)
		{
			GL.Begin(PrimitiveType.Points);
			foreach (var point in points)
			{
				var p = point * 2f - Vector2.One;
				GL.Vertex2(p.X, p.Y);
			}
			GL.End();
		}

		private void DrawLine(IEnumerable<Vector2> points)
		{
			GL.Begin(PrimitiveType.LineStrip);
			foreach (var point in points)
			{
				var p = point * 2f - Vector2.One;
				GL.Vertex2(p.X, p.Y);
			}
			GL.End();
		}
	}
}
