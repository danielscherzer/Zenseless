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

		public Camera<FirstPerson, Perspective> Camera { get; } = new Camera<FirstPerson, Perspective>(new FirstPerson(new Vector3(36, 0.1f, 30)), new Perspective(70, 0.01f, 300f));
		public bool Wireframe { get; set; } = true;

		private readonly int instanceSqrt = 100;

		public void Draw()
		{
			GL.PolygonMode(MaterialFace.FrontAndBack, Wireframe ? PolygonMode.Line : PolygonMode.Fill);
			shader.Uniform("camera", Camera);
			shader.Uniform(nameof(instanceSqrt), instanceSqrt);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.DrawArraysInstanced(PrimitiveType.Patches, 0, 4, instanceSqrt * instanceSqrt);
		}

		public void Resize(int width, int height)
		{
		}
	}
}
