namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
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
			var instanceMesh = contentLoader.Load<DefaultMesh>("suzanne").Transform(Transformation.Scale(0.01f));
			var lightData = CreateLightData(20, 20);
			var suzanneInstances = VAOLoader.FromMesh(instanceMesh, shaderMaterial);
			suzanneInstances.SetAttribute(shaderMaterial.GetResourceLocation(ShaderResourceType.Attribute, nameof(lightData)), lightData, true);
			meshVisuals.Add(new MeshVisual(suzanneInstances, shaderMaterial));

			shaderLighting = contentLoader.Load<IShaderProgram>("lighting.*");
			var lightMesh = Meshes.CreateSphere(1f, 2); // either make it bigger or you need good subdivision to avoid border artifacts
			lightGeometry = VAOLoader.FromMesh(lightMesh, shaderLighting);
			lightGeometry.SetAttribute(shaderLighting.GetResourceLocation(ShaderResourceType.Attribute, nameof(lightData)), lightData, true);
		}

		private static Vector4[] CreateLightData(uint xCount, uint zCount)
		{
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float Color() => Rnd01();

			var positions = new List<Vector4>();

			void CreateVertex(float x, float z) => positions.Add(new Vector4(x, 0.01f * Rnd01(), z, Color()));
			ShapeBuilder.Grid(-1, 2, -1, 2, xCount - 1, zCount - 1, CreateVertex, null, null, null);
			return positions.ToArray();
		}

		private readonly IRenderState renderState;

		public void Draw(ITransformation camera)
		{
			mrtSurface.Activate(); //start drawing into texture

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			renderState.Set(new DepthTest(true));
			foreach(var vis in meshVisuals) vis.Draw((s) => s.Uniform("camera", camera));
			renderState.Set(new DepthTest(false));

			mrtSurface.Deactivate(); //stop drawing into texture
			//TextureDebugger.Draw(mrtSurface.Textures.ElementAt(1)); return;

			GL.Clear(ClearBufferMask.ColorBufferBit);
			renderState.Set(BlendStates.Additive);
			renderState.Set(new FaceCullingModeState(FaceCullingMode.FRONT_SIDE)); // important to cull for light volume rendering, otherwise we would have lighting partly twice applied and to cull front face, otherwise near clipping artifacts
			DrawTools.BindTextures(shaderLighting, bindings);
			shaderLighting.Activate();
			shaderLighting.Uniform(nameof(camera), camera);
			shaderLighting.Uniform("lightIntensity", 0.2f);
			shaderLighting.Uniform("lightRange", 0.2f);
			lightGeometry.Draw();
			shaderLighting.Deactivate();
			DrawTools.UnbindTextures(bindings);
			renderState.Set(BlendStates.Opaque);
		}

		public void Resize(int width, int height)
		{
			var texNormalMaterial = Texture2dGL.Create(width, height, 4, true);
			var texPosition = Texture2dGL.Create(width, height, 4, true);

			mrtSurface = new FBOwithDepth(texNormalMaterial);
			mrtSurface.Attach(texPosition);
			bindings = new List<TextureBinding>()
			{
				new TextureBinding(nameof(texNormalMaterial), texNormalMaterial),
				new TextureBinding(nameof(texPosition), texPosition),
			};
		}

		private IRenderSurface mrtSurface;
		private IShaderProgram shaderLighting;
		private IEnumerable<TextureBinding> bindings;
		private readonly List<MeshVisual> meshVisuals = new List<MeshVisual>();
		private readonly VAO lightGeometry;
	}
}
