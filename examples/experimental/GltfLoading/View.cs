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
			//fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\BoxAnimated\glTF\BoxAnimated.gltf";
			fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\RiggedSimple\glTF\RiggedSimple.gltf";
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
					var location = shader.GetResourceLocation(ShaderResourceType.Attribute, attributeName);
					return location;
				}
				model = new GltfModelRendererGL(stream, LoadFile, UniformLoc, AttributeLoc);
			}
			Cameras = model.Cameras;
			time = new Stopwatch();
			time.Start();
		}

		internal void Draw(ITransformation camera, Vector3 cameraPos)
		{
			void SetWorld(ITransformation transform) => shader.Uniform("world", transform);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			shader.Uniform(nameof(cameraPos), cameraPos);
			shader.Uniform(nameof(camera), camera);
			model.Draw((float)time.Elapsed.TotalSeconds, SetWorld);
			shader.Deactivate();

			if (time.ElapsedMilliseconds > 5000) time.Restart();
		}		
	}
}