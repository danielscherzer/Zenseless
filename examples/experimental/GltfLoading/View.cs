using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using Zenseless.HLGL;

namespace Example
{
	internal class View
	{
		private readonly IShaderProgram shader;
		private readonly Action draw;

		public View(IContentLoader contentLoader)
		{
			shader = contentLoader.Load<IShaderProgram>("shader.*");
			//using (var stream = contentLoader.Load<Stream>("AnimatedTriangle.gltf"))
			//using (var stream = contentLoader.Load<Stream>("Box.gltf"))
			using (var stream = contentLoader.Load<Stream>("2CylinderEngine.gltf"))
			{
				var gltf = new GltfModelToGL(stream);
				draw = gltf.CreateDrawCommand(shader);
			}
		}

		internal void Draw()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shader.Activate();
			draw();
			shader.Deactivate();
		}
	}
}