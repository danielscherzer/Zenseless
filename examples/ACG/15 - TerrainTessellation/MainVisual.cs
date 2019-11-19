namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	public class MainVisual
	{
		private readonly IShaderProgram shader;

		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new DepthTest(true));

			shader = contentLoader.Load<IShaderProgram>("TerrainTessellation.*");
			shader.Activate();

			GL.PatchParameter(PatchParameterInt.PatchVertices, 4);
		}

		public float LODScale { get; set; } = 100f;
		public bool Wireframe { get; set; } = true;

		private readonly int columnCount = 100;

		public void Draw(ITransformation camera)
		{
			GL.PolygonMode(MaterialFace.FrontAndBack, Wireframe ? PolygonMode.Line : PolygonMode.Fill);
			shader.Uniform("lodScale", LODScale);
			shader.Uniform("camera", camera);
			shader.Uniform(nameof(columnCount), columnCount);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			// draw columnCount * columnCount count quad patches
			GL.DrawArraysInstanced(PrimitiveType.Patches, 0, 4, columnCount * columnCount);
		}

		public void Resize(int width, int height)
		{
		}
	}
}
