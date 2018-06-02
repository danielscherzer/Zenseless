namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;

	/// <summary>
	/// 
	/// </summary>
	public static class DrawableGL
	{
		/// <summary>
		/// Creates the draw call gl.
		/// </summary>
		/// <param name="primitiveType">Type of the primitive.</param>
		/// <param name="elementCount">The element count.</param>
		/// <param name="instanceCount">The instance count.</param>
		/// <param name="vao">The vao.</param>
		/// <returns></returns>
		public static Action CreateDrawCallGL(PrimitiveType primitiveType, int elementCount, int instanceCount, VAO vao)
		{
			if (vao is null)
			{
				void Draw()
				{
					GL.DrawArraysInstanced(primitiveType, 0, elementCount, instanceCount);
				}
				return Draw;
			}
			else
			{
				void Draw()
				{
					vao.Activate();
					GL.DrawArraysInstanced(primitiveType, 0, elementCount, instanceCount);
					vao.Deactivate();
				}
				return Draw;
			}
		}

		/// <summary>
		/// Creates the indexed draw call gl.
		/// </summary>
		/// <param name="primitiveType">Type of the primitive.</param>
		/// <param name="drawElementsType">Type of the draw elements.</param>
		/// <param name="idCount">The identifier count.</param>
		/// <param name="instanceCount">The instance count.</param>
		/// <param name="vao">The vao.</param>
		/// <returns></returns>
		public static Action CreateIndexedDrawCallGL(PrimitiveType primitiveType, DrawElementsType drawElementsType, int idCount, int instanceCount, VAO vao)
		{
			if (vao is null)
			{
				void Draw()
				{
					GL.DrawElementsInstanced(primitiveType, idCount, drawElementsType, (IntPtr)0, instanceCount);
				}
				return Draw;
			}
			else
			{
				void Draw()
				{
					vao.Activate();
					GL.DrawElementsInstanced(primitiveType, idCount, drawElementsType, (IntPtr)0, instanceCount);
					vao.Deactivate();
				}
				return Draw;
			}
		}
	}
}
