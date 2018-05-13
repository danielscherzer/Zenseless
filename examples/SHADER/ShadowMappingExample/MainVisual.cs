using OpenTK.Graphics.OpenGL4;
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
			renderState.Set(new DepthTest(true));
			renderState.Set(new BackFaceCulling(false));
			fboShadowMap.Texture.Filter = TextureFilterMode.Nearest;

			shaderProgram = contentLoader.Load<IShaderProgram>("shadowMap.*");
			var mesh = Meshes.CreatePlane(10, 10, 10, 10);
			var sphere = Meshes.CreateSphere(0.5f, 2);
			sphere.SetConstantUV(new Vector2(0.5f, 0.5f));
			mesh.Add(sphere.Transform(Transformation.Translation(0, 2, -2)));
			mesh.Add(sphere.Transform(Transformation.Translation(0, 2, 0)));
			mesh.Add(sphere.Transform(Transformation.Translation(2, 2, -1)));
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);

			shaderProgramDepth = contentLoader.Load<IShaderProgram>("depth.*");
			//todo: radeon cards created errors with geometry bound to one shader and used in other shaders because of binding id changes
		}

		public void Render(TransformationHierarchyNode camera)
		{
			if (shaderProgram is null) return;
			if (shaderProgramDepth is null) return;
			//first pass: create shadow map
#if SOLUTION
			shaderProgramDepth.Activate();
			fboShadowMap.Activate();
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			var light = cameraLight.CalcGlobalTransformation();
			shaderProgramDepth.Uniform("camera", light, true);
			geometry.Draw();
			shaderProgramDepth.Deactivate();
			fboShadowMap.Deactivate();
#endif
			//second pass: render with shadow map
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			fboShadowMap.Texture.Activate();
			shaderProgram.Uniform("ambient", new Vector3(0.1f));
			shaderProgram.Uniform("camera", camera.CalcGlobalTransformation(), true);
#if SOLUTION
			shaderProgram.Uniform(nameof(light), light, true);
#endif
			geometry.Draw();
			fboShadowMap.Texture.Deactivate();
			shaderProgram.Deactivate();
		}

		private TransformationHierarchyNode cameraLight = new Orbit(8, -100, 44, new Perspective(farClip:50));
		private IShaderProgram shaderProgram;
		private IShaderProgram shaderProgramDepth;
		private IRenderSurface fboShadowMap = new FBOwithDepth(Texture2dGL.Create(512, 512, 1, true));
		private VAO geometry;
	}
}
