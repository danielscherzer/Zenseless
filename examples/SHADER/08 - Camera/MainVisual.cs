namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Numerics;
	using Zenseless.Patterns;
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
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
			UpdateAttributes(shaderProgram);
		}

		public void Render(ITransformation camera)
		{
			if (shaderProgram is null) return;
			var time = gameTime.AbsoluteTime;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform(nameof(time), time);
			shaderProgram.Uniform("camera", camera);
			geometry.Draw();
			shaderProgram.Deactivate();
		}

		private const int particleCount = 500;
		private IShaderProgram shaderProgram;
		private GameTime gameTime = new GameTime();
		private VAO geometry;

		private void UpdateAttributes(IShaderProgram shaderProgram)
		{
			//per instance attributes
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 8.0f;
			var instancePositions = new Vector3[particleCount];
			for (int i = 0; i < particleCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, true);

			float RndSpeed() => (Rnd01() - 0.5f);
			var instanceSpeeds = new Vector3[particleCount];
			for (int i = 0; i < particleCount; ++i)
			{
				instanceSpeeds[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceSpeed"), instanceSpeeds, true);
		}
	}
}
