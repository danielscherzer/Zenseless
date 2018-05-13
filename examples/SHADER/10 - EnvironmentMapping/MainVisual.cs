using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new ClearColorState(1, 1, 1, 1));
			renderState.Set(new DepthTest(true));
			renderState.Set(new BackFaceCulling(true));

			var envMap = contentLoader.Load<ITexture2D>("beach");
			envMap.WrapFunction = TextureWrapFunction.MirroredRepeat;
			envMap.Filter = TextureFilterMode.Linear;
			var textBinding = new TextureBinding[] { new TextureBinding(nameof(envMap), envMap) };

			var shaderProgram = contentLoader.Load<IShaderProgram>("envMapping.*");

			var sphere = Meshes.CreateSphere(1f, 4);
#if SOLUTION
			visuals.Add(new MeshVisual(sphere, shaderProgram, textBinding));
#endif
			var envSphere = sphere.Transform(Transformation.Scale(200f)).SwitchTriangleMeshWinding();
			visuals.Add(new MeshVisual(envSphere, shaderProgram, textBinding));
		}

		public void Render(TransformationHierarchyNode camera, Vector3 cameraPosition)
		{
			void SetUniforms(IShaderProgram shaderProgram)
			{
				shaderProgram.Uniform("camera", camera.CalcGlobalTransformation(), true);
				shaderProgram.Uniform(nameof(cameraPosition), cameraPosition);
			}
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			foreach(var visual in visuals)
			{
				visual.Draw(SetUniforms);
			}
		}

		private readonly List<MeshVisual> visuals = new List<MeshVisual>();
	}
}
