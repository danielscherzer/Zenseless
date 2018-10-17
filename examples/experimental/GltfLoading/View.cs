using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
	internal class View
	{
		private readonly IShaderProgram shader;
		private readonly Action draw;
		private Matrix4 camera;

		public View(IContentLoader contentLoader, IRenderContext renderContext)
		{
			renderContext.RenderState.Set(new DepthTest(true));
			//renderContext.RenderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			shader = contentLoader.Load<IShaderProgram>("shader.*");
			//using (var stream = contentLoader.Load<Stream>("AnimatedTriangle.gltf"))
			//using (var stream = contentLoader.Load<Stream>("Box.gltf"))
			//using (var stream = contentLoader.Load<Stream>("2CylinderEngine.gltf"))
			//using (var stream = contentLoader.Load<Stream>("BrainStem.gltf"))
			//using (var stream = File.OpenRead(@"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\Cameras\glTF-Embedded\Cameras.gltf"))
			using (var stream = File.OpenRead(@"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\OrientationTest\glTF-Embedded\OrientationTest.gltf"))
			{
				var gltf = new GltfModelToGL(stream);
				int UniformLoc(string name) => shader.GetResourceLocation(ShaderResourceType.Uniform, name);

				int AttributeLoc(string name)
				{
					var attributeName = name.ToLowerInvariant();
					return shader.GetResourceLocation(ShaderResourceType.Attribute, attributeName);
				}
				gltf.UpdateDrawCommands(UniformLoc, AttributeLoc);
				var locWorld = shader.GetResourceLocation(ShaderResourceType.Uniform, "world");
				draw = gltf.CreateTreeDrawCommand((m) => GL.UniformMatrix4(locWorld, true, ref m));
				camera = gltf.Camera;
			}
		}

		internal void Draw(ITransformation camera)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			shader.Uniform(nameof(camera), camera);
			var locCam = shader.GetResourceLocation(ShaderResourceType.Uniform, "camera");
			//GL.UniformMatrix4(locCam, false, ref this.camera);
			draw();
			shader.Deactivate();
		}
	}
}