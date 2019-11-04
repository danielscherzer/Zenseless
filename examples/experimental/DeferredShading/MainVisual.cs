namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			this.renderState = renderState;
			var shaderMaterial = contentLoader.Load<IShaderProgram>("material.*");

			var plane = Meshes.CreatePlane(4, 4, 1, 1);
			meshVisuals.Add(new MeshVisual(plane, shaderMaterial));
			var instanceMesh = contentLoader.Load<DefaultMesh>("suzanne").Transform(Transformation.Scale(0.03f));
			var instanceData = CreateInstancePositionMaterial(20, 20);
			var suzanneInstances = VAOLoader.FromMesh(instanceMesh, shaderMaterial);
			suzanneInstances.SetAttribute(shaderMaterial.GetResourceLocation(ShaderResourceType.Attribute, nameof(instanceData)), instanceData, true);
			meshVisuals.Add(new MeshVisual(suzanneInstances, shaderMaterial));
			var shaderLighting = contentLoader.Load<IShaderProgram>("lighting.*");
			var lightMesh = Meshes.CreateSphere(1f, 2); // either make it bigger or you need good subdivision to avoid border artifacts
			var lightGeometry = VAOLoader.FromMesh(lightMesh, shaderLighting);
			lightGeometry.SetAttribute(shaderLighting.GetResourceLocation(ShaderResourceType.Attribute, "lightData"), instanceData, true);
			lightsVisual = new MeshVisual(lightGeometry, shaderLighting);
		}

		public float LightIntensity { get; set; } = 0.2f;
		public float LightRange { get; set; } = 0.2f;

		public void Draw(ITransformation camera)
		{
			var cameraUniform = new TransformUniform(nameof(camera), camera);

			mrtSurface.Execute(() =>
			{
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
				renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
				renderState.Set(new DepthTest(true));
				foreach (var vis in meshVisuals)
				{
					vis.SetUniform(cameraUniform); //TODO: only once per shader
					vis.Draw();
				}
				renderState.Set(new DepthTest(false));
			});
			//TextureDebugger.Draw(mrtSurface.Textures.ElementAt(1)); return;

			GL.Clear(ClearBufferMask.ColorBufferBit);
			renderState.Set(BlendStates.Additive);
			renderState.Set(new FaceCullingModeState(FaceCullingMode.FRONT_SIDE)); // important to cull for light volume rendering, otherwise we would have lighting partly twice applied and to cull front face, otherwise near clipping artifacts
			lightsVisual.SetUniform(cameraUniform);
			lightsVisual.Draw();
			renderState.Set(BlendStates.Opaque);
		}

		public void Resize(int width, int height)
		{
			var texNormalMaterial = Texture2dGL.Create(width, height, 4, true);
			var texPosition = Texture2dGL.Create(width, height, 4, true);

			mrtSurface = new FBOwithDepth(texNormalMaterial);
			mrtSurface.Attach(texPosition);
			var bindings = new List<TextureBinding>()
			{
				new TextureBinding(nameof(texNormalMaterial), texNormalMaterial),
				new TextureBinding(nameof(texPosition), texPosition),
			};
			lightsVisual = new MeshVisual(lightsVisual.Drawable, lightsVisual.ShaderProgram, bindings);
			lightsVisual.SetUniform(new FloatUniform("lightIntensity", LightIntensity));
			lightsVisual.SetUniform(new FloatUniform("lightRange", LightRange));
		}

		private readonly IRenderState renderState;
		private IRenderSurface mrtSurface;
		private readonly List<MeshVisual> meshVisuals = new List<MeshVisual>();
		private MeshVisual lightsVisual;

		private static Vector4[] CreateInstancePositionMaterial(uint xCount, uint zCount)
		{
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float Color() => Rnd01();

			var positions = new List<Vector4>();

			void CreateVertex(float x, float z) => positions.Add(new Vector4(x, 0.01f, z, Color()));
			ShapeBuilder.Grid(-1, 2, -1, 2, xCount - 1, zCount - 1, CreateVertex, null, null, null);
			return positions.ToArray();
		}
	}
}
