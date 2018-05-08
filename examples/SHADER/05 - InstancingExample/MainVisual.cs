using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
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
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = Meshes.CreateSphere(0.03f, 2);
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);

			//per instance attributes
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			var instancePositions = new Vector3[instanceCount];
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			//TODO students: add per instance attribute speed here
		}

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			geometry.Draw(instanceCount);
			shaderProgram.Deactivate();
		}

		private const int instanceCount = 1000;
		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
