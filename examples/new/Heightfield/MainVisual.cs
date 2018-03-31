using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Heightfield
{
	internal class MainVisual
	{
		public MainVisual(IRenderContext renderContext, IContentLoader contentLoader)
		{
			renderContext.RenderState.Set(BoolState<IDepthState>.Enabled);
			renderContext.RenderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			var mesh = Meshes.CreatePlane(2, 2, 1024, 1024);

			var texHeightfield = contentLoader.Load<ITexture2D>("mountain_height");
			var heightField = new float[texHeightfield.Width, texHeightfield.Height];
			texHeightfield.ToBuffer(ref heightField);
			var bindings = new TextureBinding[]
			{
				new TextureBinding("texHeightfield", texHeightfield),
				new TextureBinding("texColor", contentLoader.Load<ITexture2D>("mountain_color")),
				new TextureBinding("texStone", contentLoader.Load<ITexture2D>("stone")),
			};
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			mountain = new MeshVisual(mesh, shaderProgram, bindings);
			shaderProgram.Activate(); // only one shader
		}

		internal void Render(Transformation3D camera)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
			mountain.Draw();
		}

		private readonly MeshVisual mountain;
		private readonly IShaderProgram shaderProgram;
	}
}