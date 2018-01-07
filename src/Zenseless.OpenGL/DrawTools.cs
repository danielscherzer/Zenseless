using Zenseless.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	public static class DrawTools
	{
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
		/// Writes OpenGL errors to the debug output.
		/// </summary>
		public static void WriteErrors()
		{
			//check errors
			ErrorCode error;
			while (ErrorCode.NoError != (error = GL.GetError()))
			{
				Debug.WriteLine(error);
			}
		}

		/// <summary>
		/// To the open tk.
		/// </summary>
		/// <param name="v">The v.</param>
		/// <returns></returns>
		public static Vector3 ToOpenTK(this System.Numerics.Vector3 v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}

		/// <summary>
		/// To the open tk.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <returns></returns>
		public static Matrix4 ToOpenTK(this System.Numerics.Matrix4x4 m)
		{
			return new Matrix4(m.M11, m.M12, m.M13, m.M14,
				m.M21, m.M22, m.M23, m.M24,
				m.M31, m.M32, m.M33, m.M34,
				m.M41, m.M42, m.M43, m.M44);
		}
	}
}
