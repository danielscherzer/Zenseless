namespace Example
{
	using Zenseless.ExampleFramework;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;
	using OpenTK.Graphics.OpenGL4;
	using System;

	class MyVisual
	{
		private MyVisual()
		{
			string sVertexShader = @"
				#version 430 core				
				out vec3 pos; 
				void main() {
					const vec3 vertices[4] = vec3[4](vec3(-0.9, -0.8, 0.5),
                                    vec3( 0.9, -0.9, 0.5),
                                    vec3( 0.9,  0.8, 0.5),
                                    vec3(-0.9,  0.9, 0.5));
					pos = vertices[gl_VertexID];
					gl_Position = vec4(pos, 1.0);
				}";
			string sFragmentShd = @"
			#version 430 core
			in vec3 pos;
			out vec4 color;
			void main() {
				color = vec4(pos + 1.0, 1.0);
			}";
			//read shader from file
			//string fileName = "Hello world.glsl";
			//try
			//{
			//	using (StreamReader sr = new StreamReader(fileName))
			//	{
			//		sFragmentShd = sr.ReadToEnd();
			//		sr.Dispose();
			//	}
			//}
			//catch { };
			shaderProgram = new ShaderProgramGL();
			shaderProgram.Compile(sVertexShader, Zenseless.HLGL.ShaderType.VertexShader);
			shaderProgram.Compile(sFragmentShd, Zenseless.HLGL.ShaderType.FragmentShader);
			shaderProgram.Link();
		}

		private IShaderProgram shaderProgram;

		private void Render()
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shaderProgram.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderProgram.Deactivate();
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual();
			window.Render += visual.Render;
			window.Run();

		}
	}
}