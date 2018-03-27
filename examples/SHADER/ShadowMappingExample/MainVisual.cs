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
			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Disabled);
			fboShadowMap.Texture.Filter = TextureFilterMode.Nearest;

			shaderProgram = contentLoader.Load<IShaderProgram>("shadowMap.*");
			var mesh = Meshes.CreatePlane(10, 10, 10, 10);
			var sphere = Meshes.CreateSphere(0.5f, 2);
			sphere.SetConstantUV(new System.Numerics.Vector2(0.5f, 0.5f));
			mesh.Add(sphere.Transform(new Translation3D(0, 2, -2)));
			mesh.Add(sphere.Transform(new Translation3D(0, 2, 0)));
			mesh.Add(sphere.Transform(new Translation3D(2, 2, -1)));
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
			shaderProgramDepth = contentLoader.Load<IShaderProgram>("depth.*");

		}

		public void Render(Transformation3D camera)
		{
			if (shaderProgram is null) return;
			if (shaderProgramDepth is null) return;
			//TODO student: first pass: create shadow map

			//second pass: render with shadow map
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			fboShadowMap.Texture.Activate();
			shaderProgram.Uniform("ambient", new Vector3(0.1f));
			shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
			geometry.Draw();
			fboShadowMap.Texture.Deactivate();
			shaderProgram.Deactivate();
		}

		private Transformation3D cameraLight = new Orbit(8, -100, 44, new Perspective(farClip:50));
		private IShaderProgram shaderProgram;
		private IShaderProgram shaderProgramDepth;
		private FBO fboShadowMap = new FBOwithDepth(Texture2dGL.Create(512, 512, 1, true));
		private VAO geometry;
	}
}
