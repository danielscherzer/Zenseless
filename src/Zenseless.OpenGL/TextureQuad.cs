namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System.Globalization;
	using System.Threading;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// Draw a texture on a quad
	/// </summary>
	public class TextureQuad
	{
		/// <summary>Initializes a new instance of the <see cref="TextureQuad"/> class.</summary>
		/// <param name="screenBounds">The screen bounds.</param>
		/// <param name="colorMappingGlslExpression">A color mapping GLSL expression.</param>
		public TextureQuad(Box2D screenBounds, string colorMappingGlslExpression = "color = color;")
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // for correct float conversion
			string sVertexShader = @"
				#version 130
				out vec2 uv; 
				void main() 
				{
					const vec2 vertices[4] = vec2[4](vec2(0.0, 0.0),
						vec2( 1.0, 0.0),
						vec2( 1.0,  1.0),
						vec2( 0.0,  1.0));

					uv = vertices[gl_VertexID];
					vec2 pos = REPLACE;
					gl_Position = vec4(pos, -1.0, 1.0);
				}";
			sVertexShader = sVertexShader.Replace("REPLACE"
				, $"vec2({screenBounds.MinX}, {screenBounds.MinY}) + uv * vec2({screenBounds.SizeX}, {screenBounds.SizeY})");
			string sFragmentShd = @"#version 430 core
				uniform sampler2D image;
				in vec2 uv;
				out vec4 fragColor;
				void main() 
				{
					vec4 color = texture(image, uv);
					colorMappingExpression
					fragColor = color;
				}";
			sFragmentShd = sFragmentShd.Replace("colorMappingExpression", colorMappingGlslExpression);
			shaderProgram = ShaderLoader.CreateFromStrings(sVertexShader, sFragmentShd);
		}

		/// <summary>
		/// Draws the specified texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		public void Draw(ITexture2D texture)
		{
			texture.Activate();
			shaderProgram.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderProgram.Deactivate();
			texture.Deactivate();
		}

		private ShaderProgramGL shaderProgram;
	}
}