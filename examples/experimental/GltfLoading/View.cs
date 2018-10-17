using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
	internal class View
	{
		private readonly IShaderProgram shader;
		private readonly Action draw;
		private readonly IEnumerable<Matrix4> cameras;

		public View(IContentLoader contentLoader, IRenderContext renderContext)
		{
			renderContext.RenderState.Set(new DepthTest(true));
			//renderContext.RenderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			shader = contentLoader.Load<IShaderProgram>("shader.*");

			//var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\Cameras\glTF-Embedded\Cameras.gltf";
			var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\BrainStem\glTF\BrainStem.gltf";
			//var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\OrientationTest\glTF-Embedded\OrientationTest.gltf";
			var directory = Path.GetDirectoryName(fileName);
			//using (var stream = contentLoader.Load<Stream>("AnimatedTriangle.gltf"))
			//using (var stream = contentLoader.Load<Stream>("Box.gltf"))
			//using (var stream = contentLoader.Load<Stream>("2CylinderEngine.gltf"))
			//using (var stream = contentLoader.Load<Stream>("BrainStem.gltf"))
			using (var stream = File.OpenRead(fileName))
			{
				var gltf = new GltfModelToGL(stream, (externalFile) => File.ReadAllBytes(Path.Combine(directory, externalFile)));
				int UniformLoc(string name) => shader.GetResourceLocation(ShaderResourceType.Uniform, name);

				int AttributeLoc(string name)
				{
					var attributeName = name.ToLowerInvariant();
					return shader.GetResourceLocation(ShaderResourceType.Attribute, attributeName);
				}
				gltf.UpdateDrawCommands(UniformLoc, AttributeLoc);
				var locWorld = shader.GetResourceLocation(ShaderResourceType.Uniform, "world");
				draw = gltf.CreateTreeDrawCommand((m) => GL.UniformMatrix4(locWorld, false, ref m));
				cameras = gltf.Cameras;
			}
		}

		internal void Draw(ITransformation camera)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			if (0 == cameras.Count())
			{
				shader.Uniform(nameof(camera), camera);
			}
			else
			{
				var locCam = shader.GetResourceLocation(ShaderResourceType.Uniform, "camera");
				var cam = cameras.First();
				GL.UniformMatrix4(locCam, false, ref cam);
			}
			draw();
			shader.Deactivate();
		}
	}
}