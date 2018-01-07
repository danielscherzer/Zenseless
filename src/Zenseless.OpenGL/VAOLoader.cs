using Zenseless.Geometry;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Provides static methods for VertexArrayObject data loading
	/// </summary>
	public static class VAOLoader
	{
		/// <summary>
		/// Creates a VertexArrayObject from a mesh expecting the MeshAttribute names as shader variable names for the attributes 
		/// </summary>
		/// <param name="mesh">From which to load positions, indices, normals, texture coordinates</param>
		/// <param name="shader">Used for the attribute location bindings</param>
		/// <returns>A vertex array object</returns>
		public static VAO FromMesh(DefaultMesh mesh, IShader shader)
		{
			var vao = new VAO(PrimitiveType.Triangles);
			if (mesh.Position.Count > 0)
			{
				var loc = shader.GetResourceLocation(ShaderResourceType.Attribute, DefaultMesh.PositionName);
				vao.SetAttribute(loc, mesh.Position.ToArray(), VertexAttribPointerType.Float, 3);
			}
			if (mesh.Normal.Count > 0)
			{
				var loc = shader.GetResourceLocation(ShaderResourceType.Attribute, DefaultMesh.NormalName);
				vao.SetAttribute(loc, mesh.Normal.ToArray(), VertexAttribPointerType.Float, 3);
			}
			if (mesh.TexCoord.Count > 0)
			{
				var loc = shader.GetResourceLocation(ShaderResourceType.Attribute, DefaultMesh.TexCoordName);
				vao.SetAttribute(loc, mesh.TexCoord.ToArray(), VertexAttribPointerType.Float, 2);
			}
			vao.SetIndex(mesh.IDs.ToArray());
			vao.PrimitiveType = PrimitiveType.Triangles;
			return vao;
		}
	}
}
