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
			renderState.Set(new DepthTest(true));
			renderState.Set(new FaceCullingModeState(FaceCullingMode.BACK_SIDE));

			shaderProgramPhong = contentLoader.Load<IShaderProgram>("phong.*");
			var mesh = new DefaultMesh();
			var roomSize = 8;
			//off-center plane
			var plane = Meshes.CreatePlane(roomSize, roomSize, 2, 2).Transform(Transformation.Translation(0, -roomSize / 2, 0));
			mesh.Add(plane);
			//rotate plane to create box
			mesh.Add(plane.Transform(Transformation.Rotation(90f, Axis.Z)));
			mesh.Add(plane.Transform(Transformation.Rotation(180f, Axis.Z)));
			mesh.Add(plane.Transform(Transformation.Rotation(270f, Axis.Z)));
			mesh.Add(plane.Transform(Transformation.Rotation(90f, Axis.X)));
			mesh.Add(plane.Transform(Transformation.Rotation(-90f, Axis.X)));

			var sphere = Meshes.CreateSphere(1);
			sphere.SetConstantUV(Vector2.Zero); //all other meshes have texture coordinates
			mesh.Add(sphere);
			var suzanne = contentLoader.Load<DefaultMesh>("suzanne");
			mesh.Add(suzanne.Transform(Transformation.Translation(2, 2, -2)));
			geometryPhong = VAOLoader.FromMesh(mesh, shaderProgramPhong);

			shaderProgramToon = contentLoader.Load<IShaderProgram>("toon.*");
			geometryToon = VAOLoader.FromMesh(suzanne.Transform(Transformation.Translation(2, 0, 0)), shaderProgramToon);
		}

		public void Render(ITransformation camera, in Vector3 cameraPosition)
		{
			if (shaderProgramPhong is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			shaderProgramPhong.Activate();
			shaderProgramPhong.Uniform("light1Direction", Vector3.Normalize(new Vector3(-1, -1, 1)));
			shaderProgramPhong.Uniform("light1Color", new Vector4(1f, 0f, 0f, 1f));
			shaderProgramPhong.Uniform(nameof(light2Position), light2Position);
			shaderProgramPhong.Uniform(nameof(light2Color), light2Color);
			shaderProgramPhong.Uniform("light3Position", new Vector3(-2, 2, 2));
			shaderProgramPhong.Uniform("light3Direction", Vector3.Normalize(new Vector3(1, -1, -1)));
			shaderProgramPhong.Uniform("light3Angle", MathHelper.DegreesToRadians(10f));
			shaderProgramPhong.Uniform("light3Color", new Vector4(0, 0, 1f, 1f));
			shaderProgramPhong.Uniform(nameof(ambientLightColor), ambientLightColor);
			shaderProgramPhong.Uniform(nameof(materialColor), materialColor);
			shaderProgramPhong.Uniform(nameof(camera), camera);
			shaderProgramPhong.Uniform(nameof(cameraPosition), cameraPosition);
			geometryPhong.Draw();
			shaderProgramPhong.Deactivate();

			shaderProgramToon.Activate();
			shaderProgramToon.Uniform(nameof(light2Color), light2Color);
			shaderProgramToon.Uniform(nameof(light2Position), light2Position);
			shaderProgramToon.Uniform(nameof(ambientLightColor), ambientLightColor);
			shaderProgramToon.Uniform(nameof(materialColor), materialColor);
			shaderProgramToon.Uniform(nameof(camera), camera);
			shaderProgramToon.Uniform(nameof(cameraPosition), cameraPosition);
			geometryToon.Draw();
			shaderProgramToon.Deactivate();

		}

		private readonly IShaderProgram shaderProgramPhong;
		private readonly IShaderProgram shaderProgramToon;
		private readonly IDrawable geometryPhong;
		private readonly IDrawable geometryToon;
		private readonly Vector4 ambientLightColor = new Vector4(.3f, .3f, .1f, 1f);
		private readonly Vector4 materialColor = new Vector4(.7f, .7f, .7f, 1f);
		private readonly Vector3 light2Position = new Vector3(1, 1, 4);
		private readonly Vector4 light2Color = new Vector4(1f, 1f, 1f, 1f);
	}
}
