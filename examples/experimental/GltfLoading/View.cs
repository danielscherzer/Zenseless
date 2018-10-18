using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
	internal class View
	{
		private readonly IShaderProgram shader;
		private readonly Action draw;

		public Box3D Bounds { get; private set; }

		public IEnumerable<Matrix4> Cameras { get; }

		public View(IContentLoader contentLoader, IRenderContext renderContext)
		{
			renderContext.RenderState.Set(new DepthTest(true));
			//renderContext.RenderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			shader = contentLoader.Load<IShaderProgram>("shader.*");

			//var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\Cameras\glTF-Embedded\Cameras.gltf";
			var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\BrainStem\glTF\BrainStem.gltf";
			//var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\2CylinderEngine\glTF\2CylinderEngine.gltf";
			//var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\OrientationTest\glTF-Embedded\OrientationTest.gltf";
			var directory = Path.GetDirectoryName(fileName);
			//using (var stream = contentLoader.Load<Stream>("AnimatedTriangle.gltf"))
			//using (var stream = contentLoader.Load<Stream>("Box.gltf"))
			using (var stream = File.OpenRead(fileName))
			{
				var gltf = new GltfModelToGL(stream, (externalFile) => File.ReadAllBytes(Path.Combine(directory, externalFile)));
				var size = gltf.Max - gltf.Min;
				Bounds = new Box3D(gltf.Min[0], gltf.Min[1], gltf.Min[2], size[0], size[1], size[2]);
				int UniformLoc(string name) => shader.GetResourceLocation(ShaderResourceType.Uniform, name);

				int AttributeLoc(string name)
				{
					var attributeName = name.ToLowerInvariant();
					return shader.GetResourceLocation(ShaderResourceType.Attribute, attributeName);
				}
				gltf.UpdateDrawCommands(UniformLoc, AttributeLoc);
				var locWorld = shader.GetResourceLocation(ShaderResourceType.Uniform, "world");
				draw = gltf.CreateTreeDrawCommand((m) => GL.UniformMatrix4(locWorld, false, ref m));
				Cameras = gltf.Cameras;
			}
		}

		internal void Draw(ITransformation camera, System.Numerics.Vector3 cameraPos)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			shader.Uniform(nameof(cameraPos), cameraPos);
			shader.Uniform(nameof(camera), camera);
			draw();
			shader.Deactivate();
		}		
	}
}