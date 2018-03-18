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
			camera.Distance = 1.8f;
			camera.TargetY = -0.3f;
			camera.FovY = 70;

			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = Meshes.CreateCornellBox();
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
			bufferMaterials.Set(Meshes.CreateCornellBoxMaterial(), BufferUsageHint.StaticDraw);
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "ambient"), new Vector3(0.1f));
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "lightPosition"), new Vector3(0, 0.9f, -0.5f));
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "lightColor"), new Vector3(0.8f));
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "cameraPosition"), camera.CalcPosition().ToOpenTK());
			var bindingIndex = shaderProgram.GetResourceLocation(ShaderResourceType.UniformBuffer, "bufferMaterials");
			bufferMaterials.ActivateBind(bindingIndex);
			geometry.Draw();
			bufferMaterials.Deactivate();
			shaderProgram.Deactivate();
		}

		private CameraOrbit camera = new CameraOrbit();
		private BufferObject bufferMaterials = new BufferObject(BufferTarget.UniformBuffer);
		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
