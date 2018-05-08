using System.Numerics;
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

		public void Draw(in Matrix4x4 cam)
		{
			if (shdPlane is null) return;

			renderState.Set(BoolState<IBackfaceCullingState>.Disabled);
			shdPlane.Activate();
			shdPlane.Uniform("camera", cam);

			plane.Draw();
			shdPlane.Deactivate();
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
		}

		private VAO plane;
		private IShaderProgram shdPlane;
		private readonly IRenderState renderState;
	}
}