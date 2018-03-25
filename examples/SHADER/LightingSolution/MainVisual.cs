using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			camera.FarClip = 50;
			camera.Distance = 15;
			camera.FovY = 30;

			renderState.Set(BoolState<IDepthState>.Enabled);
			renderState.Set(BoolState<IBackfaceCullingState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("phong.*");
			var mesh = new DefaultMesh();
			var roomSize = 8;
			var plane = Meshes.CreatePlane(roomSize, roomSize, 2, 2);
			var xFormCenter = new Translation3D(0, -roomSize / 2, 0);
			mesh.Add(plane.Transform(xFormCenter));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 90f, xFormCenter)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 180f, xFormCenter)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 270f, xFormCenter)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.X, 90f, xFormCenter)));
			mesh.Add(plane.Transform(new Rotation3D(Axis.X, -90f, xFormCenter)));

			var sphere = Meshes.CreateSphere(1);
			sphere.SetConstantUV(new System.Numerics.Vector2(0, 0));
			mesh.Add(sphere);
			var suzanne = contentLoader.Load<DefaultMesh>("suzanne");
			mesh.Add(suzanne.Transform(new Translation3D(2, 2, -2)));
			geometry = VAOLoader.FromMesh(mesh, shaderProgram);
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shaderProgram.Activate();
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light1Direction"), new Vector3(-1, -1, 1).Normalized());
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light1Color"), new Color4(1f, 0f, 0f, 1f));
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light2Position"), new Vector3(1, 1, 1));
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light2Color"), new Color4(1f, 1f, 1f, 1f));
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light3Position"), new Vector3(-2, 2, 2));
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light3Direction"), new Vector3(1, -1, -1).Normalized());
			GL.Uniform1(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light3Angle"), Zenseless.Geometry.MathHelper.DegreesToRadians(10f));
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "light3Color"), new Color4(0, 0, 1f, 1f));
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "ambientLightColor"), new Color4(.3f, .3f, .1f, 1f));
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "materialColor"), new Color4(.7f, .7f, .7f, 1f));
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform3(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "cameraPosition"), camera.CalcPosition().ToOpenTK());
			geometry.Draw();
			shaderProgram.Deactivate();
		}

		private CameraOrbit camera = new CameraOrbit();

		private IShaderProgram shaderProgram;
		private VAO geometry;
	}
}
