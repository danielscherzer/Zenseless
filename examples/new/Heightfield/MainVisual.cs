using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Heightfield
{
	internal class MainVisual
	{
		public MainVisual(IRenderContext renderContext, IContentLoader contentLoader)
		{
			Camera.NearClip = 0.01f;
			Camera.FarClip = 20f;
			Camera.FovY = 70f;
			Camera.Position = new Vector3(0, 0.5f, 1);

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

		public CameraFirstPerson Camera { get; private set; } = new CameraFirstPerson();

		internal void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			void SetUniforms(Func<string, int> GetLocation)
			{
				ShaderProgramGL.Uniform(GetLocation("camera"), Camera.CalcMatrix());
			}
			mountain.Draw(SetUniforms);
		}

		private readonly MeshVisual mountain;
	}
}