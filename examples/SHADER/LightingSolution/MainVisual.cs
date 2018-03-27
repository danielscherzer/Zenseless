namespace Example
{
	using OpenTK.Graphics.OpenGL4;
	using System.Numerics;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("phong.*");
			var mesh = new DefaultMesh();
			var roomSize = 8;
			//off-center plane
			var plane = Meshes.CreatePlane(roomSize, roomSize, 2, 2).Transform(new Translation3D(0, -roomSize / 2, 0));
			mesh.Add(plane);
			//rotate plane to create box
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 90f)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 180f)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 270f)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.X, 90f)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.X, -90f)));

			var sphere = Meshes.CreateSphere(1);
			sphere.SetConstantUV(new Vector2(0, 0));
			mesh.Add(sphere);
			var suzanne = contentLoader.Load<DefaultMesh>("suzanne");
			mesh.Add(suzanne.Transform(new Translation3D(2, 2, -2)));
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public void Render(Transformation3D camera, Vector3 cameraPosition)
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			shaderProgram.Uniform("light1Direction", Vector3.Normalize(new Vector3(-1, -1, 1)));
			shaderProgram.Uniform("light1Color", new Vector4(1f, 0f, 0f, 1f));
			shaderProgram.Uniform("light2Position", new Vector3(1, 1, 1));
			shaderProgram.Uniform("light2Color", new Vector4(1f, 1f, 1f, 1f));
			shaderProgram.Uniform("light3Position", new Vector3(-2, 2, 2));
			shaderProgram.Uniform("light3Direction", Vector3.Normalize(new Vector3(1, -1, -1)));
			shaderProgram.Uniform("light3Angle", MathHelper.DegreesToRadians(10f));
			shaderProgram.Uniform("light3Color", new Vector4(0, 0, 1f, 1f));
			shaderProgram.Uniform("ambientLightColor", new Vector4(.3f, .3f, .1f, 1f));
			shaderProgram.Uniform("materialColor", new Vector4(.7f, .7f, .7f, 1f));
			shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
			shaderProgram.Uniform("cameraPosition", cameraPosition);
			geometry.Draw();
			shaderProgram.Deactivate();
		}

		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
