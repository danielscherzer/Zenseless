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
			camera.FarClip = 500;
			camera.Distance = 30;

			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			geometryBody = VAOLoader.FromMesh(mesh, shaderProgram);

			var plane = Meshes.CreatePlane(100, 100, 10, 10);
			var xForm = new Transformation();
			xForm.TranslateLocal(0, -20, 0);
			geometryPlane = VAOLoader.FromMesh(plane.Transform(xForm), shaderProgram);
			//for AMD graphics cards
			geometryPlane.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), new Vector3[] { Vector3.Zero }, VertexAttribPointerType.Float, 3, true);
			geometryPlane.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceScale"), new float[] { 1 }, VertexAttribPointerType.Float, 1, true);
		}

		public CameraOrbit Camera { get { return camera; } }

		public void Render(IEnumerable<IBody> bodies, float time)
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
			GL.Uniform1(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "time"), time);
			Matrix4 cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			geometryBody.Draw(instancePositions.Count);
			//geometryPlane.Draw(); //todo student: uncomment for gravity
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private VAO geometryBody, geometryPlane;
		private CameraOrbit camera = new CameraOrbit();
	}
}
