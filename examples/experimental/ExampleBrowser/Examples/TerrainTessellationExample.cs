namespace Example
{
	using ExampleBrowser;
	using OpenTK.Graphics.OpenGL4;
	using System.ComponentModel.Composition;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	[ExampleDisplayName]
	[Export(typeof(IExample))]
	public class TerrainTessellationExample : Example
	{
		private readonly IShaderProgram shader;

		[ImportingConstructor]
		public TerrainTessellationExample([Import] IRenderState renderState, [Import] IContentLoader contentLoader)
		{
			renderState.Set(new DepthTest(true));

			shader = contentLoader.Load<IShaderProgram>("TerrainTessellation.*");
			shader.Activate();

			//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 4);
		}

		public Camera<FirstPerson, Perspective> Camera { get; } = new Camera<FirstPerson, Perspective>(new FirstPerson(new Vector3(36, 0.1f, 30)), new Perspective(70, 0.01f, 300f));
		private readonly int instanceSqrt = 100;

		public override void Render()
		{
			shader.Uniform("camera", Camera);
			shader.Uniform(nameof(instanceSqrt), instanceSqrt);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.DrawArraysInstanced(PrimitiveType.Patches, 0, 4, instanceSqrt * instanceSqrt);
		}
	}
}
