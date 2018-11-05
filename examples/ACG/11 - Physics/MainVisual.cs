﻿namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new DepthTest(true));
			renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			geometryBody = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public void Render(IEnumerable<IBody> bodies, float time, ITransformation camera)
		{
			if (shaderProgram is null) return;
			var instancePositions = new List<Vector3>();
			var instanceScale = new List<float>();
			foreach (var body in bodies)
			{
				instancePositions.Add(body.Location);
				instanceScale.Add((float)Math.Pow(body.Mass, 0.33f));
			}
			geometryBody.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions.ToArray(), true);
			geometryBody.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceScale"), instanceScale.ToArray(), true);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform(nameof(time), time);
			shaderProgram.Uniform("camera", camera);
			geometryBody.Draw();
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private VAO geometryBody;
	}
}
