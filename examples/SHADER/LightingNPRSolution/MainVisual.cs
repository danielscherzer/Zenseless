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
			renderState.Set(new ClearColorState(1, 1, 1, 1));
			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			//mesh.Add(Meshes.CreateSphere(.7f, 3));
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public void Render(Transformation3D camera, in Vector3 cameraPosition)
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform("lightColor", new Vector4(1f, 1f, 1f, 1f));
			shaderProgram.Uniform("lightPosition", new Vector3(1, 1, 4));
			shaderProgram.Uniform("ambientLightColor", new Vector4(.2f, .2f, .2f, 1f));
			shaderProgram.Uniform("materialColor", new Vector4(1f, .5f, .5f, 1f));
			shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
			shaderProgram.Uniform("cameraPosition", cameraPosition);

			geometry.Draw();
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
