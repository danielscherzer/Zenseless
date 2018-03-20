using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class VisualPlane
	{
		public VisualPlane(IRenderState renderState, IContentLoader contentLoader)
		{
			shdPlane = contentLoader.Load<IShaderProgram>("plane.*");
			var mesh = Meshes.CreatePlane(2, 2, 1, 1);
			plane = VAOLoader.FromMesh(mesh, shdPlane);
			this.renderState = renderState;
		}

		public void Draw(Matrix4 cam)
		{
			if (shdPlane is null) return;

			renderState.Set(BoolState<IBackfaceCullingState>.Disabled);
			shdPlane.Activate();
			GL.UniformMatrix4(shdPlane.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);

			plane.Draw();
			shdPlane.Deactivate();
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
		}

		private VAO plane;
		private IShaderProgram shdPlane;
		private readonly IRenderState renderState;
	}
}