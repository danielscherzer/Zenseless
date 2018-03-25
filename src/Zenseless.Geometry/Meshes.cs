using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public static partial class Meshes
	{
		/// <summary>
		/// Sets the constant uv.
		/// </summary>
		/// <param name="mesh">The mesh.</param>
		/// <param name="uv">The uv.</param>
		public static void SetConstantUV(this DefaultMesh mesh, Vector2 uv)
		{
			var uvs = mesh.TexCoord;
			var pos = mesh.Position;
			uvs.Capacity = pos.Count;
			//overwrite existing
			for(int i = 0; i < uvs.Count; ++i)
			{
				uvs[i] = uv;
			}
			//add
			for(int i = uvs.Count; i < pos.Count; ++i)
			{
				uvs.Add(uv);
			}
		}

		/// <summary>
		/// Adds the specified b.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <exception cref="ArgumentException">Original mesh has no normals, but added mesh has normals
		/// or
		/// Original mesh has no uvs, but added mesh has uvs</exception>
		public static void Add(this DefaultMesh a, DefaultMesh b)
		{
			var count = (uint)a.Position.Count;
			a.Position.AddRange(b.Position);
			if(b.Normal.Count > 0)
			{
				if (a.Normal.Count != count) throw new ArgumentException("Original mesh has no normals, but added mesh has normals");
				a.Normal.AddRange(b.Normal);
			}
			if (b.TexCoord.Count > 0)
			{
				if (a.TexCoord.Count != count) throw new ArgumentException("Original mesh has no uvs, but added mesh has uvs");
				a.TexCoord.AddRange(b.TexCoord);
			}
			foreach(var id in b.IDs)
			{
				a.IDs.Add(id + count);
			}
		}

		/// <summary>
		/// Transforms the mesh by the specified transform.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <param name="transform">The transformation matrix.</param>
		/// <returns></returns>
		public static DefaultMesh Transform(this DefaultMesh m, Matrix4x4 transform)
		{
			var mesh = new DefaultMesh();
			mesh.TexCoord.AddRange(m.TexCoord);
			mesh.IDs.AddRange(m.IDs);
			foreach (var pos in m.Position)
			{
				var newPos = Vector3.Transform(pos, transform);
				mesh.Position.Add(newPos);
			}
			foreach (var n in m.Normal)
			{
				var newN = Vector3.Normalize(Vector3.TransformNormal(n, transform));
				mesh.Normal.Add(newN);
			}
			return mesh;
		}

		/// <summary>
		/// Transforms the mesh by the specified transform.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <param name="transform">The transformation.</param>
		/// <returns></returns>
		public static DefaultMesh Transform(this DefaultMesh m, Transformation3D transform)
		{
			if (transform is null) throw new ArgumentNullException(nameof(transform));
			return Transform(m, transform.GetLocalToWorld());
		}

		/// <summary>
		/// Switches the handedness.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <returns></returns>
		public static DefaultMesh SwitchHandedness(this DefaultMesh m)
		{
			var mesh = new DefaultMesh();
			mesh.TexCoord.AddRange(m.TexCoord);
			mesh.IDs.AddRange(m.IDs);
			foreach (var pos in m.Position)
			{
				var newPos = pos;
				newPos.Z = -newPos.Z;
				mesh.Position.Add(newPos);
			}
			foreach (var n in m.Normal)
			{
				var newN = n;
				newN.Z = -newN.Z;
				mesh.Normal.Add(newN);
			}
			return mesh;
		}

		/// <summary>
		/// Flips the normals.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <returns></returns>
		public static DefaultMesh FlipNormals(this DefaultMesh m)
		{
			var mesh = new DefaultMesh();
			mesh.Position.AddRange(m.Position);
			mesh.TexCoord.AddRange(m.TexCoord);
			mesh.IDs.AddRange(m.IDs);
			foreach (var n in m.Normal)
			{
				var newN = -n;
				mesh.Normal.Add(newN);
			}
			return mesh;
		}

		/// <summary>
		/// Switches the triangle mesh winding.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <returns></returns>
		public static DefaultMesh SwitchTriangleMeshWinding(this DefaultMesh m)
		{
			var mesh = new DefaultMesh();
			mesh.Position.AddRange(m.Position);
			mesh.Normal.AddRange(m.Normal);
			mesh.TexCoord.AddRange(m.TexCoord);
			for (int i = 0; i < m.IDs.Count; i += 3)
			{
				mesh.IDs.Add(m.IDs[i]);
				mesh.IDs.Add(m.IDs[i + 2]);
				mesh.IDs.Add(m.IDs[i + 1]);
			}
			return mesh;
		}

		/// <summary>
		/// Creates the cornell box.
		/// </summary>
		/// <param name="roomSize">Size of the room.</param>
		/// <param name="sphereRadius">The sphere radius.</param>
		/// <param name="cubeSize">Size of the cube.</param>
		/// <returns></returns>
		public static DefaultMesh CreateCornellBox(float roomSize = 2, float sphereRadius = 0.3f, float cubeSize = 0.6f)
		{
			var mesh = new DefaultMesh();
			var plane = CreatePlane(roomSize, roomSize, 2, 2);

			var xFormCenter = new Translation3D(0, -roomSize / 2, 0);
			plane.SetConstantUV(new Vector2(3, 0));
			mesh.Add(plane.Transform(xFormCenter));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 90f, xFormCenter)));
			plane.SetConstantUV(new Vector2(1, 0));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 180f, xFormCenter)));
			plane.SetConstantUV(new Vector2(0, 0));
			mesh.Add(plane.Transform(new Rotation3D(Axis.Z, 270f, xFormCenter)));
			plane.SetConstantUV(new Vector2(2, 0));
			mesh.Add(plane.Transform(new Rotation3D(Axis.X, 90f, xFormCenter)));
			plane.SetConstantUV(new Vector2(0, 0));
			mesh.Add(plane.Transform(new Rotation3D(Axis.X, -90f, xFormCenter)));

			var sphere = Meshes.CreateSphere(sphereRadius, 4);
			sphere.SetConstantUV(new Vector2(3, 0));
			mesh.Add(sphere.Transform(new Translation3D(0.4f, -1 + sphereRadius, -0.2f)));

			var cube = Meshes.CreateCubeWithNormals(cubeSize);
			var trans = new Translation3D(-0.5f, -1 + 0.5f * cubeSize, 0.1f);
			var translateAndRotY = new Rotation3D(Axis.Y, 35f, trans);

			cube.SetConstantUV(new Vector2(3, 0));
			mesh.Add(cube.Transform(translateAndRotY));
			return mesh;
		}
		/// <summary>
		/// 
		/// </summary>
		public struct CornellBoxMaterial //use 16 byte alignment or you have to query all variable offsets
		{
			/// <summary>
			/// The color
			/// </summary>
			public Vector3 color;
			/// <summary>
			/// The shininess
			/// </summary>
			public float shininess;
		};
		/// <summary>
		/// Creates the cornell box material.
		/// </summary>
		/// <returns></returns>
		public static CornellBoxMaterial[] CreateCornellBoxMaterial()
		{
			var materials = new CornellBoxMaterial[4];
			materials[0].color = new Vector3(1, 1, 1);
			materials[0].shininess = 0;
			materials[1].color = new Vector3(0, 1, 0);
			materials[1].shininess = 0;
			materials[2].color = new Vector3(1, 0, 0);
			materials[2].shininess = 0;
			materials[3].color = new Vector3(1, 1, 1);
			materials[3].shininess = 256;
			return materials;
		}

		/// <summary>
		/// Creates a cube made up of pairs of triangles; stored as an indexed vertex array
		/// </summary>
		/// <param name="size">length of one side</param>
		/// <returns>
		/// Mesh with positions, ids, normals
		/// </returns>
		public static DefaultMesh CreateCubeWithNormals(float size = 1.0f)
		{
			var m = new DefaultMesh();
			void createPosition(float x, float y, float z) => m.Position.Add(new Vector3(x, y, z));
			void createID(uint index) => m.IDs.Add(index);
			void createNormal(float x, float y, float z) => m.Normal.Add(new Vector3(x, y, z));
			ShapeBuilder.Cube(createPosition, createID, size, createNormal);
			return m;
		}

		/// <summary>
		/// creates a sphere made up of pairs of triangles; stored as an indexed vertex array
		/// </summary>
		/// <param name="radius">radius</param>
		/// <param name="subdivision">subdivision count, each subdivision creates 4 times more faces</param>
		/// <returns>
		/// Mesh with positions, ids, normals
		/// </returns>
		public static DefaultMesh CreateSphere(float radius = 1.0f, uint subdivision = 1)
		{
			var m = new DefaultMesh();
			//var pos = m.AddAttribute<Vector3>(Mesh.PositionName);
			//var normal = m.AddAttribute<Vector3>(Mesh.NormalName);
			void createPosition(float x, float y, float z) => m.Position.Add(new Vector3(x, y, z));
			void createID(uint id) => m.IDs.Add(id);
			void createNormal(float x, float y, float z) => m.Normal.Add(new Vector3(x, y, z));
			ShapeBuilder.Sphere(createPosition, createID, radius, subdivision, createNormal);
			return m;
		}

		/// <summary>
		/// creates an icosahedron made up of pairs of triangles; stored as an indexed vertex array
		/// </summary>
		/// <param name="radius">radius</param>
		/// <returns>
		/// Mesh with positions, ids, normals
		/// </returns>
		public static DefaultMesh CreateIcosahedron(float radius)
		{
			return CreateSphere(radius, 0);
		}

		/// <summary>
		/// Creates a plane made up of pairs of triangles; stored as an indexed vertex array.
		/// </summary>
		/// <param name="sizeX">extent of the grid in the x-coordinate axis</param>
		/// <param name="sizeZ">extent of the grid in the z-coordinate axis</param>
		/// <param name="segmentsX">number of grid segments in the x-coordinate axis</param>
		/// <param name="segmentsZ">number of grid segments in the z-coordinate axis</param>
		/// <returns>
		/// Mesh with positions, ids, normals, and uvs
		/// </returns>
		public static DefaultMesh CreatePlane(float sizeX, float sizeZ, uint segmentsX, uint segmentsZ)
		{
			var m = new DefaultMesh();
			void CreateVertex(float x, float z) => m.Position.Add(new Vector3(x, 0.0f, z));
			void CreateID(uint id) => m.IDs.Add(id);
			void CreateNormal() => m.Normal.Add(Vector3.UnitY);
			void CreateUV(float u, float v) => m.TexCoord.Add(new Vector2(u, v));

			var startX = -sizeX / 2f;
			var startZ= -sizeZ / 2f;
			ShapeBuilder.Grid(startX, sizeX, startZ, sizeZ, segmentsX, segmentsZ, CreateVertex, CreateID
				, CreateNormal, CreateUV);
			return m;
		}
	}
}
