using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.Base;
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
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
			UpdateAttributes(shaderProgram);
		}

		public void Render(Transformation3D camera)
		{
			if (shaderProgram is null) return;
			var time = gameTime.AbsoluteTime;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform("time", time);
			shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
			geometry.Draw(particelCount);
			shaderProgram.Deactivate();
		}

		private const int particelCount = 500;
		private IShaderProgram shaderProgram;
		private GameTime gameTime = new GameTime();
		private VAO geometry;

		private void UpdateAttributes(IShaderProgram shaderProgram)
		{
			//per instance attributes
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 8.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			float RndSpeed() => (Rnd01() - 0.5f);
			var instanceSpeeds = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instanceSpeeds[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceSpeed"), instanceSpeeds, VertexAttribPointerType.Float, 3, true);
		}
	}
}
