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

			var shader = contentLoader.Load<IShaderProgram>("shader.*");
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
			mountain = new MeshVisual(mesh, shader, bindings);
		}

		internal void Render(Transformation3D camera)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			void SetUniforms(Func<string, int> GetLocation)
			{
				ShaderProgramGL.Uniform(GetLocation("camera"), camera.CalcLocalToWorldColumnMajorMatrix());
			}
			mountain.Draw(SetUniforms);
		}

		private readonly MeshVisual mountain;
	}
}