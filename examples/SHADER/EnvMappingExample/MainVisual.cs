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

			envMap = contentLoader.Load<ITexture2D>("beach");
			envMap.WrapFunction = TextureWrapFunction.MirroredRepeat;
			envMap.Filter = TextureFilterMode.Linear;

			shaderProgram = contentLoader.Load<IShaderProgram>("envMapping.*");

			var sphere = Meshes.CreateSphere(1, 4);
			var envSphere = sphere.SwitchTriangleMeshWinding();
			//var refSphere = sphere.
			geometry = VAOLoader.FromMesh(envSphere, shaderProgram);
		}

		public void Render(Transformation3D camera, Vector3 cameraPosition)
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			envMap.Activate();
			shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
			shaderProgram.Uniform("cameraPosition", cameraPosition);
			geometry.Draw();
			envMap.Deactivate();
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private ITexture envMap;
		private VAO geometry;
	}
}
