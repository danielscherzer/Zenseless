using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			camera.FarClip = 50;
			camera.Distance = 8;
			camera.Elevation = 30;

			cameraLight.FarClip = 50;
			cameraLight.Distance = 8;
			cameraLight.Elevation = 44;
			cameraLight.Azimuth = -100;

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

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Render()
		{
			if (shaderProgram is null) return;
			if (shaderProgramDepth is null) return;
			//TODO student: first pass: create shadow map

			//second pass: render with shadow map
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			fboShadowMap.Texture.Activate();
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "ambient"), new Vector3(0.1f));
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			geometry.Draw();
			fboShadowMap.Texture.Deactivate();
			shaderProgram.Deactivate();
		}

		private CameraOrbit camera = new CameraOrbit();
		private CameraOrbit cameraLight = new CameraOrbit();
		private IShaderProgram shaderProgram;
		private IShaderProgram shaderProgramDepth;
		private FBO fboShadowMap = new FBOwithDepth(Texture2dGL.Create(512, 512, 1, true));
		private VAO geometry;
	}
}
