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

		private readonly int locJoints;

		public IEnumerable<Transformation> Cameras { get; }

		private readonly Stopwatch time;

		public View(IContentLoader contentLoader, IRenderContext renderContext)
		{
			renderContext.RenderState.Set(new DepthTest(true));
			//renderContext.RenderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			shader = contentLoader.Load<IShaderProgram>("shader.*");

			var fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\Cameras\glTF-Embedded\Cameras.gltf";
			fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\BrainStem\glTF\BrainStem.gltf";
			fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\BoxAnimated\glTF\BoxAnimated.gltf";
			fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\RiggedSimple\glTF\RiggedSimple.gltf";
			fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\RiggedFigure\glTF\RiggedFigure.gltf";
			//fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\Monster\glTF\Monster.gltf";
			fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\CesiumMan\glTF\CesiumMan.gltf";
			//fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\2CylinderEngine\glTF\2CylinderEngine.gltf";
			//fileName = @"D:\Daten\downloads\gits\glTF-Sample-Models\2.0\OrientationTest\glTF-Embedded\OrientationTest.gltf";
			//fileName = @"D:\Daten\downloads\_cg\gltf models\ferris_wheel_animated\scene.gltf";
			fileName = @"D:\Daten\downloads\_cg\gltf models\izzy_-_animated_female_character_free_download\scene.gltf";
			//fileName = @"D:\Daten\downloads\_cg\gltf models\littlest_tokyo\scene.gltf";
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
			locJoints = shader.GetResourceLocation(ShaderResourceType.Uniform, "u_jointMat");
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

			void SetJointMatrices(Matrix4x4[] jointTransformations) => GL.UniformMatrix4(locJoints, jointTransformations.Length, false, jointTransformations.ToFloatArray());

			model.UpdateAnimations((float)time.Elapsed.TotalSeconds, SetJointMatrices);

			void SetWorld(ITransformation transform) => shader.Uniform("world", transform);

			if (model.IsSkinned)
			{
				var joints = model.CalculateJointTransforms();
				SetJointMatrices(joints);
				SetWorld(Transformation.Identity);
				model.Draw((m) => { });
				model.Draw(SetWorld);
			}
			else
			{
				//initialize joint matrices with identity
				var ident = new Matrix4x4[100];
				for (int i = 0; i < ident.Length; ++i)
				{
					ident[i] = Matrix4x4.Identity;
				}
				float[] buffer = ident.ToFloatArray();
				SetJointMatrices(ident);
				model.Draw(SetWorld);
			}

			shader.Deactivate();

			if (time.ElapsedMilliseconds > 5000) time.Restart();
		}		
	}
}