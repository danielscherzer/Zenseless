using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
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
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			geometryBody = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public void Render(IEnumerable<IBody> bodies, float time, Transformation3D camera)
		{
			if (shaderProgram is null) return;
			var instancePositions = new List<Vector3>();
			var instanceScale = new List<float>();
			foreach (var body in bodies)
			{
				instancePositions.Add(body.Location);
				instanceScale.Add((float)Math.Pow(body.Mass, 0.33f));
			}
			geometryBody.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions.ToArray(), VertexAttribPointerType.Float, 3, true);
			geometryBody.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceScale"), instanceScale.ToArray(), VertexAttribPointerType.Float, 1, true);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform("time", time);
			shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
			geometryBody.Draw(instancePositions.Count);
			//geometryPlane.Draw(); //todo student: uncomment for gravity
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private VAO geometryBody;
	}
}
