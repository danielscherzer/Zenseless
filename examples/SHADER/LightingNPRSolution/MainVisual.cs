using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			camera.FarClip = 20;
			camera.Distance = 5;
			camera.FovY = 30;

			renderState.Set(new ClearColorState(1, 1, 1, 1));
			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			//mesh.Add(Meshes.CreateSphere(.7f, 3));
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "lightColor"), new Color4(1f, 1f, 1f, 1f));
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "lightPosition"), new Vector3(1, 1, 4));
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "ambientLightColor"), new Color4(.2f, .2f, .2f, 1f));
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "materialColor"), new Color4(1f, .5f, .5f, 1f));
			Matrix4 cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "cameraPosition"), camera.CalcPosition().ToOpenTK());

			geometry.Draw();
			shaderProgram.Deactivate();
		}

		private CameraOrbit camera = new CameraOrbit();
		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
