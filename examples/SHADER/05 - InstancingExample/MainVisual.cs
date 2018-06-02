namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(new DepthTest(true));
			renderState.Set(new BackFaceCulling(true));

			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = Meshes.CreateSphere(0.03f, 2);
#if SOLUTION
			mesh = contentLoader.Load<DefaultMesh>("suzanne").Transform(Transformation.Scale(0.03f));
#endif
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);

			InitParticles();
		}

		public void Render(ITransformation camera)
		{
#if SOLUTION
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePositions[i] += instanceVelocity[i];
				var abs = Vector3.One - Vector3.Abs(instancePositions[i]);
				if (abs.X < 0 || abs.Y < 0 || abs.Z < 0)
				{
					instanceVelocity[i] = -instanceVelocity[i];
				}
				instanceRotation[i] += 0.1f;
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, true);
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceRotation"), instanceRotation, true);
#endif

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
#if SOLUTION
			shaderProgram.Uniform("camera", camera);
#endif
			geometry.Draw();
			shaderProgram.Deactivate();
		}

		private const int instanceCount = 10000;
		private readonly Vector3[] instancePositions = new Vector3[instanceCount];
#if SOLUTION
		private readonly Vector3[] instanceVelocity = new Vector3[instanceCount];
		private readonly float[] instanceRotation = new float[instanceCount];
		private readonly Vector3[] instanceColor = new Vector3[instanceCount];
#endif
		private IShaderProgram shaderProgram;
		private VAO geometry;

		private void InitParticles()
		{
			//per instance attributes
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, true);

#if SOLUTION
			float RndVelocity() => (Rnd01() - 0.5f) * 0.01f;
			for (int i = 0; i < instanceCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
				instanceVelocity[i] = new Vector3(RndVelocity(), RndVelocity(), RndVelocity());
				instanceColor[i] = new Vector3(0.5f) + instancePositions[i] * 0.5f;
				instanceRotation[i] = Rnd01() * MathHelper.TWO_PI;
			}
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceColor"), instanceColor, true);
#endif
		}
	}
}
