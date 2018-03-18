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
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			var mesh = contentLoader.Load<DefaultMesh>("suzanne");
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public void Render()
		{
			if (shaderProgram is null) return;

			//Matrix4 is stored row-major -> implies a transpose so in shader matrix is column major
			var loc = shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceTransform");
			geometry.SetAttribute(loc, instanceTransforms, true);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			Matrix4 camera = Matrix4.CreateScale(1, 1, -1);
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref camera);
			geometry.Draw(instanceTransforms.Length);
			shaderProgram.Deactivate();
		}

		public void Update(float time)
		{
			//store matrices as per instance attributes
			//Matrix4 transforms are row-major -> transforms are written T1*T2*...
			for (int i = 0; i < instanceTransforms.Length; ++i)
			{
				instanceTransforms[i] = Matrix4.CreateScale(0.2f);
			}
			instanceTransforms[0] *= Matrix4.CreateScale((float)Math.Sin(time) * 0.5f + 0.7f);
			instanceTransforms[1] *= Matrix4.CreateTranslation(0, (float)Math.Sin(time) * 0.7f, 0);
			instanceTransforms[2] *= Matrix4.CreateRotationY(time);
			for (int i = 0; i < instanceTransforms.Length; ++i)
			{
				instanceTransforms[i] *= Matrix4.CreateTranslation((i - 1) * 0.65f, 0, 0);
			}
		}

		private Matrix4[] instanceTransforms = new Matrix4[3];
		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
