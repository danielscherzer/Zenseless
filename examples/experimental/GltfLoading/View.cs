using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
	internal class View
	{
		private readonly IShaderProgram shader;
		private readonly GltfModelRendererGL model;

		public Box3D Bounds => model.Bounds;

		public IEnumerable<Transformation> Cameras { get; }

		private readonly Stopwatch time;

		public View(IContentLoader contentLoader, IRenderContext renderContext)
		{
			renderContext.RenderState.Set(new DepthTest(true));
			//renderContext.RenderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			shader = contentLoader.Load<IShaderProgram>("shader.*");

			var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\Cameras\glTF-Embedded\Cameras.gltf";
			fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\BrainStem\glTF\BrainStem.gltf";
			//fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\2CylinderEngine\glTF\2CylinderEngine.gltf";
			//fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\OrientationTest\glTF-Embedded\OrientationTest.gltf";
			var directory = Path.GetDirectoryName(fileName);
			//using (var stream = contentLoader.Load<Stream>("AnimatedTriangle.gltf"))
			//using (var stream = contentLoader.Load<Stream>("Box.gltf"))
			using (var stream = File.OpenRead(fileName))
			{
				byte[] LoadFile(string externalFile) => File.ReadAllBytes(Path.Combine(directory, externalFile));
				int UniformLoc(string name) => shader.GetResourceLocation(ShaderResourceType.Uniform, name);
				int AttributeLoc(string name)
				{
					var attributeName = name.ToLowerInvariant();
					return shader.GetResourceLocation(ShaderResourceType.Attribute, attributeName);
				}
				//var locWorld = shader.GetResourceLocation(ShaderResourceType.Uniform, "world");
				void SetWorld(ITransformation transform) => shader.Uniform("world", transform);

				model = new GltfModelRendererGL(stream, LoadFile, UniformLoc, AttributeLoc, SetWorld);
			}
			Cameras = model.Cameras;
			time = new Stopwatch();
			time.Start();
		}

		internal void Draw(ITransformation camera, Vector3 cameraPos)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			shader.Uniform(nameof(cameraPos), cameraPos);
			shader.Uniform(nameof(camera), camera);
			model.Draw((float)time.Elapsed.TotalSeconds);
			shader.Deactivate();
		}		
	}
}