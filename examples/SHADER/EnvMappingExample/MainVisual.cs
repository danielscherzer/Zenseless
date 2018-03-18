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
			renderState.Set(new ClearColorState(1, 1, 1, 1));
			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			envMap = contentLoader.Load<ITexture2D>("beach");
			envMap.WrapFunction = TextureWrapFunction.MirroredRepeat;
			envMap.Filter = TextureFilterMode.Linear;

			shaderProgram = contentLoader.Load<IShaderProgram>("envMapping.*");

			var sphere = Meshes.CreateSphere(1, 4);
			var envSphere = sphere.SwitchTriangleMeshWinding();
			//var refSphere = sphere.
			geometry = VAOLoader.FromMesh(envSphere, shaderProgram);
			camera.NearClip = 0.01f;
			camera.FarClip = 50;
			camera.Distance = 0;
			camera.FovY = 70;
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			envMap.Activate();
			camera.FovY = MathHelper.Clamp(camera.FovY, 0.1f, 175f);
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "cameraPosition"), camera.CalcPosition().ToOpenTK());
			geometry.Draw();
			envMap.Deactivate();
			shaderProgram.Deactivate();
		}

		private CameraOrbit camera = new CameraOrbit();
		private IShaderProgram shaderProgram;
		private ITexture envMap;
		private VAO geometry;
	}
}
