namespace Example
{
	using ExampleBrowser;
	using OpenTK.Graphics.OpenGL4;
	using System.ComponentModel.Composition;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.Patterns;

	[ExampleDisplayName]
	[Export(typeof(IExample))]
	public class TessellationExample : Example
	{
		private readonly IShaderProgram shader;
		private readonly ITime time;
		private float _tesselationLevelInner = 1f;
		private float _tesselationLevelOuter = 1f;

		[ImportingConstructor]
		public TessellationExample([Import] IContentLoader contentLoader, [Import] ITime time)
		{
			shader = contentLoader.Load<IShaderProgram>("Tessellation.*");
			shader.Activate();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 4);
			this.time = time;
		}

		public float TesselationLevelInner
		{
			get => _tesselationLevelInner;
			set => Set(ref _tesselationLevelInner, MathHelper.Clamp(value, 1, 64));
		}

		public float TesselationLevelOuter
		{
			get => _tesselationLevelOuter;
			set => Set(ref _tesselationLevelOuter, MathHelper.Clamp(value, 1, 64));
		}

		public bool Animate { get; set; } = false;

		public override void Update()
		{
			if (Animate)
			{
				TesselationLevelInner += time.DeltaTime;
				TesselationLevelOuter += time.DeltaTime;
			}
		}

		public override void Render()
		{
			shader.Uniform("tesselationLevelOuter", TesselationLevelOuter);
			shader.Uniform("tesselationLevelInner", TesselationLevelInner);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.DrawArrays(PrimitiveType.Patches, 0, 4);
		}
	}
}
