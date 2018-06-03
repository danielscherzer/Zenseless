namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.HLGL;

	/// <summary>
	/// Quickly draw a texture full-screen
	/// </summary>
	public class TextureDebugger
	{
		/// <summary>
		/// Draws the specified texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		public static void Draw(ITexture2D texture)
		{
			if (shaderProgram is null) InitShader();
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			texture.Activate();
			shaderProgram.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderProgram.Deactivate();
			texture.Deactivate();
		}

		private static ShaderProgramGL shaderProgram;

		private static void InitShader()
		{
			const string sVertexShader = @"
				#version 130
				out vec2 uv; 
				void main() 
				{
					const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0),
						vec2( 1.0, -1.0),
						vec2( 1.0,  1.0),
						vec2(-1.0,  1.0));

					vec2 pos = vertices[gl_VertexID];
					uv = pos * 0.5 + 0.5;
					gl_Position = vec4(pos, 0.0, 1.0);
				}";
			const string sFragmentShd = @"#version 430 core
				uniform sampler2D image;
				in vec2 uv;
				void main() 
				{
					gl_FragColor = abs(texture(image, uv));
				}";
			shaderProgram = new ShaderProgramGL();
			shaderProgram.Compile(sVertexShader, Zenseless.HLGL.ShaderType.VertexShader);
			shaderProgram.Compile(sFragmentShd, Zenseless.HLGL.ShaderType.FragmentShader);
			shaderProgram.Link();
		}
	}
}