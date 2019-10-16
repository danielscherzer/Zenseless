namespace Heightfield
{
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	internal class MainVisual
	{
		public MainVisual(IRenderContext renderContext, IContentLoader contentLoader)
		{
			renderContext.RenderState.Set(new DepthTest(true));
			renderContext.RenderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));

			var mesh = Meshes.CreatePlane(2, 2, 1024, 1024);

			var texHeightfield = contentLoader.Load<ITexture2D>("mountain_height");
			var bindings = new TextureBinding[]
			{
				new TextureBinding("texHeightfield", texHeightfield),
				new TextureBinding("texColor", contentLoader.Load<ITexture2D>("mountain_color")),
				new TextureBinding("texStone", contentLoader.Load<ITexture2D>("stone")),
			};
			var shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			mountain = new MeshVisual(mesh, shaderProgram, bindings);
		}

		internal void Render(ITransformation camera)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			mountain.SetUniform(new TransformUniform(nameof(camera), camera));
			mountain.Draw();
		}

		private readonly MeshVisual mountain;
	}
}