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

		public void Draw(in ITransformation camera)
		{
			if (shdPlane is null) return;

			renderState.Set(new BackFaceCulling(false));
			shdPlane.Activate();
			shdPlane.Uniform(nameof(camera), camera);

			plane.Draw();
			shdPlane.Deactivate();
			renderState.Set(new BackFaceCulling(true));
		}

		private VAO plane;
		private IShaderProgram shdPlane;
		private readonly IRenderState renderState;
	}
}