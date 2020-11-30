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
			// scene
			var plane = Meshes.CreatePlane(4, 4, 1, 1);
			meshVisuals.Add(new MeshVisual(plane, shaderMaterial));
			var instanceMesh = contentLoader.Load<DefaultMesh>("suzanne").Transform(Transformation.Scale(0.03f));
			var instanceData = CreatePerInstancePositionMaterial(20, 20);
			var suzanneInstances = VAOLoader.FromMesh(instanceMesh, shaderMaterial);
			suzanneInstances.SetAttribute(shaderMaterial.GetResourceLocation(ShaderResourceType.Attribute, nameof(instanceData)), instanceData, true);
			meshVisuals.Add(new MeshVisual(suzanneInstances, shaderMaterial));
			// lights
			var shaderLighting = contentLoader.Load<IShaderProgram>("lighting.*");
			var lightSphereMesh = Meshes.CreateSphere(1f, 2); // either make it bigger or you need good subdivision to avoid border artifacts
			var vaoInstancedLightSphere = VAOLoader.FromMesh(lightSphereMesh, shaderLighting);
			vaoInstancedLightSphere.SetAttribute(shaderLighting.GetResourceLocation(ShaderResourceType.Attribute, "lightData"), instanceData, true);
			instancedLightSphereVisual = new MeshVisual(vaoInstancedLightSphere, shaderLighting);
			//overlays
			overlayNormals = new TextureQuad(new Box2D(-1f, -1f, 0.3f, 0.3f), "color.a = 1.0;");
			overlayMaterial = new TextureQuad(new Box2D(-0.7f, -1f, 0.3f, 0.3f), "color = vec4(vec3(color.w), 1.0);");
			overlayPositions = new TextureQuad(new Box2D(-0.4f, -1f, 0.3f, 0.3f), "color.rgb = abs(color.rgb);");
		}

		public float LightIntensity { get; set; } = 0.2f;
		public float LightRange { get; set; } = 0.2f;

		public void Draw(ITransformation camera)
		{
			var cameraUniform = new TransformUniform(nameof(camera), camera);

			mrtSurface.Draw(() =>
			{
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
				renderState.Set(new DepthTest(true));
				foreach (var vis in meshVisuals)
				{
					vis.SetUniform(cameraUniform); //TODO: only once per shader
					vis.Draw();
				}
				renderState.Set(new DepthTest(false));
			});

			GL.Clear(ClearBufferMask.ColorBufferBit);
			renderState.Set(BlendStates.Additive);
			renderState.Set(new FaceCullingModeState(FaceCullingMode.FRONT_SIDE)); // important to cull for light volume rendering, otherwise we would have lighting partly twice applied and to cull front face, otherwise near clipping artifacts
			instancedLightSphereVisual.SetUniform(new FloatUniform("lightIntensity", LightIntensity));
			instancedLightSphereVisual.SetUniform(new FloatUniform("lightRange", LightRange));
			instancedLightSphereVisual.SetUniform(cameraUniform);
			instancedLightSphereVisual.Draw();
			renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			renderState.Set(BlendStates.Opaque);

			overlayNormals.Draw(mrtSurface.Textures.ElementAt(0));
			overlayMaterial.Draw(mrtSurface.Textures.ElementAt(0));
			overlayPositions.Draw(mrtSurface.Textures.ElementAt(1));
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
			instancedLightSphereVisual = new MeshVisual(instancedLightSphereVisual.Drawable, instancedLightSphereVisual.ShaderProgram, bindings);
		}

		private readonly IRenderState renderState;
		private IRenderSurface mrtSurface;
		private readonly List<MeshVisual> meshVisuals = new List<MeshVisual>();
		private MeshVisual instancedLightSphereVisual;
		private readonly TextureQuad overlayNormals;
		private readonly TextureQuad overlayMaterial;
		private readonly TextureQuad overlayPositions;

		private static Vector4[] CreatePerInstancePositionMaterial(uint xCount, uint zCount)
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
