namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using Zenseless.Geometry;
	using Zenseless.HLGL;

	/// <summary>
	/// Provides static methods for VertexArrayObject data loading
	/// </summary>
	public static class VAOLoader
	{
		/// <summary>
		/// Creates a VertexArrayObject from a mesh expecting the MeshAttribute names as shader variable names for the attributes 
		/// </summary>
		/// <param name="mesh">From which to load positions, indices, normals, texture coordinates</param>
		/// <param name="shaderProgram">Used for the attribute location bindings</param>
		/// <returns>A vertex array object</returns>
		public static VAO FromMesh(Mesh mesh, IShaderProgram shaderProgram)
		{
			var vao = new VAO(PrimitiveType.Triangles);
			foreach (var attributeName in mesh.AttributeNames)
			{
				var loc = shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, attributeName);
				var attribute = mesh.GetAttribute(attributeName);
				var array = attribute.ToArray(); // copy
				vao.SetAttribute(loc, array, attribute.BaseType, attribute.BaseTypeCount);
			}
			vao.SetIndex(mesh.IDs.ToArray());
			return vao;
		}
	}
}
