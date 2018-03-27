namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL;
	using System;
	using System.Drawing;
	using Zenseless.Base;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// 
	/// </summary>
	public static class DrawTools
	{
		/// <summary>
		/// Draws a circle. This is a slow immediate mode helper function.
		/// </summary>
		/// <param name="centerX">The center x.</param>
		/// <param name="centerY">The center y.</param>
		/// <param name="radius">The radius.</param>
		/// <param name="corners">The segments.</param>
		/// <param name="isFilled">Filled or border</param>
		public static void DrawCircle(float centerX, float centerY, float radius, int corners, bool isFilled = true)
		{
			float delta = 2f * (float)Math.PI / corners;
			var type = isFilled ? PrimitiveType.Polygon : PrimitiveType.LineLoop;
			GL.Begin(type);
			for (float alpha = 0.0f; alpha < 2 * Math.PI; alpha += delta)
			{
				float x = radius * (float)Math.Cos(alpha);
				float y = radius * (float)Math.Sin(alpha);
				GL.Vertex2(centerX + x, centerY + y);
			}
			GL.End();
		}

		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="rect">The rectangle coordinates.</param>
		/// <param name="texCoords">The rectangle texture coordinates.</param>
		public static void DrawTexturedRect(this IReadOnlyBox2D rect, IReadOnlyBox2D texCoords)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(texCoords.MinX, texCoords.MinY); GL.Vertex2(rect.MinX, rect.MinY);
			GL.TexCoord2(texCoords.MaxX, texCoords.MinY); GL.Vertex2(rect.MaxX, rect.MinY);
			GL.TexCoord2(texCoords.MaxX, texCoords.MaxY); GL.Vertex2(rect.MaxX, rect.MaxY);
			GL.TexCoord2(texCoords.MinX, texCoords.MaxY); GL.Vertex2(rect.MinX, rect.MaxY);
			GL.End();
		}

		/// <summary>
		/// Writes OpenGL errors to the console.
		/// </summary>
		public static void WriteErrors()
		{
			//check errors
			ErrorCode error;
			while (ErrorCode.NoError != (error = GL.GetError()))
			{
				Console.WriteLine($"{DebugTools.GetSourcePositionForConsoleRef()}: OpenGL Error '{error}'");
			}
		}

		/// <summary>
		/// Set color Uniform on active shader. The correct shader has to be activated first!
		/// </summary>
		/// <param name="shaderProgram">The shader program.</param>
		/// <param name="name">The uniform variable name.</param>
		/// <param name="color">The color.</param>
		public static void Uniform(this IShaderProgram shaderProgram, string name, Color color)
		{
			GL.Uniform4(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, name), color);

		}
	}
}
