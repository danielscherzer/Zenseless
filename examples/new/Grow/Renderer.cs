using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Zenseless.HLGL;

namespace Example
{
	public class Renderer
	{
		public Renderer(IRenderState renderState, IContentLoader contentLoader)
		{
			rendererPoints = new RendererPoints(contentLoader);
			renderState.Set(BlendStates.Additive);
			renderState.Set(new PointSprite(true));
			renderState.Set(new ShaderPointSize(true));
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void Resize(int width, int height)
		{
			rendererPoints.Resize(width, height);
		}

		public void DrawElements(IEnumerable<IElement> elements)
		{
			//marshall data for gpu
			var coords = new Vector3[elements.Count()];
			int i = 0;
			foreach (var element in elements)
			{
				coords[i] = Convert(element);
				++i;
			}
			rendererPoints.DrawPoints(coords, .05f, Color.CornflowerBlue);
		}

		public void DrawPlayer(IElement element)
		{
			rendererPoints.DrawPoints(new Vector3[] { Convert(element) }, .05f, Color.Red);
		}

		private RendererPoints rendererPoints;

		private static Vector3 Convert(IElement element)
		{
			return new Vector3(element.Coord, (1f - element.Size));
		}
	}
}
