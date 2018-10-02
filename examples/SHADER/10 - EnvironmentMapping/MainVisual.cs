namespace Example
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
			renderState.Set(new ClearColorState(1, 1, 1, 1));
			renderState.Set(new DepthTest(true));
			renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));

			var envMap = contentLoader.Load<ITexture2D>("beach");
			envMap.WrapFunction = TextureWrapFunction.MirroredRepeat;
			envMap.Filter = TextureFilterMode.Linear;
			var textureBinding = new TextureBinding[] { new TextureBinding(nameof(envMap), envMap) };

			var sphere = Meshes.CreateSphere(1f, 4);

			var skySphere = sphere.Transform(Transformation.Scale(200f)).SwitchTriangleMeshWinding();
			var shaderBackground = contentLoader.Load<IShaderProgram>("background.*");
			visuals.Add(new MeshVisual(skySphere, shaderBackground, textureBinding));
			//drawCalls.Add((t, pos) =>
			//);

#if SOLUTION
			var shaderEnvMap = contentLoader.Load<IShaderProgram>("envMapping.*");
			var visSphere = new MeshVisual(sphere, shaderEnvMap, textureBinding);
			visSphere.SetUniform(new FloatUniform("reflective", 1f));
			visuals.Add(visSphere);
			var suzanne = contentLoader.Load<DefaultMesh>("suzanne");
			var visSuzanne = new MeshVisual(suzanne.Transform(Transformation.Translation(0, -1.5f, 0)), shaderEnvMap, textureBinding);
			visSuzanne.SetUniform(new FloatUniform("reflective", 0.4f));
			visuals.Add(visSuzanne);
#endif
		}

		public void Render(ITransformation camera, Vector3 cameraPosition)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			//var camera = new 
			var cameraPos = new Vec3Uniform(nameof(cameraPosition), cameraPosition);
			foreach (var visual in visuals)
			{
				var shader = visual.ShaderProgram;
				shader.Uniform(nameof(camera), camera);
				shader.Uniform(nameof(cameraPosition), cameraPosition);
				visual.Draw();
			}
		}

		private readonly List<MeshVisual> visuals = new List<MeshVisual>();
		private readonly List<Action<ITransformation, Vector3>> drawCalls = new List<Action<ITransformation, Vector3>>();
	}
}
