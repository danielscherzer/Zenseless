using System;
using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// static class that provides geometric shape builder methods
	/// </summary>
	public static class ShapeBuilder
	{
		/// <summary>
		/// Creates a circle shape out of points on the circumference.
		/// </summary>
		/// <param name="createPosition">Callback for each position creation</param>
		/// <param name="centerX">The circle center x-coordinate.</param>
		/// <param name="centerY">The circle center y-coordinate.</param>
		/// <param name="radius">The circle radius.</param>
		/// <param name="count">Circumference point count</param>
		/// <exception cref="ArgumentNullException">createPosition</exception>
		public static void Circle(Action<float, float> createPosition, float centerX, float centerY, float radius, int count)
		{
			if (createPosition is null) throw new ArgumentNullException(nameof(createPosition) + " must not be null");
			var delta = MathHelper.TWO_PI / count;
			for (float alpha = 0.0f; alpha < MathHelper.TWO_PI; alpha += delta)
			{
				float x = radius * (float)Math.Cos(alpha);
				float y = radius * (float)Math.Sin(alpha);
				createPosition(centerX + x, centerY + y);
			}
		}
		/// <summary>
		/// Builds a cube made up of triangles
		/// </summary>
		/// <param name="createPosition">callback for each position creation</param>
		/// <param name="createID">callback for each index creation</param>
		/// <param name="size">length of one side</param>
		/// <param name="createNormal">callback for each vertex normal creation</param>
		public static void Cube(Action<float, float, float> createPosition, Action<uint> createID, float size = 1.0f
			, Action<float, float, float> createNormal = null)
		{
			if (createPosition is null) throw new ArgumentNullException(nameof(createPosition) + " must not be null");
			if (createID is null) throw new ArgumentNullException(nameof(createID) + " must not be null");
			float s2 = size * 0.5f;

			//corners
			var c = new Vector3[] {
				new Vector3(s2, s2, -s2),
				new Vector3(s2, s2, s2),
				new Vector3(-s2, s2, s2),
				new Vector3(-s2, s2, -s2),
				new Vector3(s2, -s2, -s2),
				new Vector3(-s2, -s2, -s2),
				new Vector3(-s2, -s2, s2),
				new Vector3(s2, -s2, s2),
			};

			uint id = 0;
			var n = -Vector3.UnitX;
			Action<int> Add = (int pos) =>
			{
				var p = c[pos];
				createPosition(p.X, p.Y, p.Z);
				createNormal(n.X, n.Y, n.Z);
				createID(id);
				++id;
			};

			if (createNormal is null)
			{
				//no normals
				Add = (int pos) => { createID(id); ++id; }; //add only ids
				foreach (var p in c) createPosition(p.X, p.Y, p.Z); //add corners once
			}

			//Left face
			Add(2);
			Add(5);
			Add(6);
			Add(2);
			Add(3);
			Add(5);
			//Right face
			n = Vector3.UnitX;
			Add(1);
			Add(4);
			Add(0);
			Add(1);
			Add(7);
			Add(4);
			//Top Face
			n = Vector3.UnitY;
			Add(0);
			Add(2);
			Add(1);
			Add(0);
			Add(3);
			Add(2);
			//Bottom Face
			n = -Vector3.UnitY;
			Add(4);
			Add(6);
			Add(5);
			Add(4);
			Add(7);
			Add(6);
			//Front Face
			n = Vector3.UnitZ;
			Add(1);
			Add(6);
			Add(7);
			Add(1);
			Add(2);
			Add(6);
			//Back Face
			n = -Vector3.UnitZ;
			Add(0);
			Add(5);
			Add(3);
			Add(0);
			Add(4);
			Add(5);
		}

		/// <summary>
		/// creates a grid shape made up of pairs of triangles; stored as an indexed vertex array.
		/// </summary>
		/// <param name="startX">start coordinate of the grid in the first coordinate axis</param>
		/// <param name="sizeX">extent of the grid in the first coordinate axis</param>
		/// <param name="startY">start coordinate of the grid in the second coordinate axis</param>
		/// <param name="sizeY">extent of the grid in the second coordinate axis</param>
		/// <param name="segmentsX">number of grid segments in the first coordinate axis</param>
		/// <param name="segmentsY">number of grid segments in the second coordinate axis</param>
		/// <param name="createPosition">callback for each position creation</param>
		/// <param name="createID">callback for each index creation</param>
		/// <param name="createNormal">callback for each vertex normal creation</param>
		/// <param name="createUV">callback for each vertex texture coordinate creation</param>
		public static void Grid(float startX, float sizeX, float startY, float sizeY
			, uint segmentsX, uint segmentsY
			, Action<float, float> createPosition, Action<uint> createID
			, Action createNormal = null, Action<float, float> createUV = null)
		{
			if (createPosition is null) throw new ArgumentNullException(nameof(createPosition) + " must not be null");
			float deltaU = (1.0f / segmentsX);
			float deltaV = (1.0f / segmentsY);
			float deltaX = deltaU * sizeX;
			float deltaY = deltaV * sizeY;
			//create vertex data
			for (uint u = 0; u <= segmentsX; ++u)
			{
				for (uint v = 0; v <= segmentsY; ++v)
				{
					float pX = startX + u * deltaX;
					float pY = startY + v * deltaY;
					createPosition(pX, pY);
					createNormal?.Invoke();
					createUV?.Invoke(u * deltaU, v * deltaV);
				}
			}
			if (createID is null) return;
			uint verticesZ = segmentsY + 1;
			//create ids
			for (uint u = 0; u < segmentsX; ++u)
			{
				for (uint v = 0; v < segmentsY; ++v)
				{
					createID(u * verticesZ + v);
					createID(u * verticesZ + v + 1);
					createID((u + 1) * verticesZ + v);

					createID((u + 1) * verticesZ + v);
					createID(u * verticesZ + v + 1);
					createID((u + 1) * verticesZ + v + 1);
				}
			}
		}

		/// <summary>
		/// Builds a sphere made up of triangles
		/// </summary>
		/// <param name="createPosition">callback for each position creation</param>
		/// <param name="createID">callback for each index creation</param>
		/// <param name="radius_">radius of the sphere</param>
		/// <param name="subdivision">subdivision count, each subdivision creates 4 times more faces</param>
		/// <param name="createNormal">callback for each vertex normal creation</param>
		public static void Sphere(Action<float, float, float> createPosition, Action<uint> createID, float radius_ = 1.0f, uint subdivision = 1
			, Action<float, float, float> createNormal = null)
		{
			if (createPosition is null) throw new ArgumentNullException(nameof(createPosition) + " must not be null");
			if (createID is null) throw new ArgumentNullException(nameof(createID) + " must not be null");
			//idea: subdivide icosahedron
			const float X = 0.525731112119133606f;
			const float Z = 0.850650808352039932f;

			var vdata = new float[12, 3] {
				{ -X, 0.0f, Z}, { X, 0.0f, Z}, { -X, 0.0f, -Z }, { X, 0.0f, -Z },
				{ 0.0f, Z, X }, { 0.0f, Z, -X }, { 0.0f, -Z, X }, { 0.0f, -Z, -X },
				{ Z, X, 0.0f }, { -Z, X, 0.0f }, { Z, -X, 0.0f }, { -Z, -X, 0.0f }
			};
			var tindices = new uint[20, 3] {
				{ 0, 1, 4 }, { 0, 4, 9 }, { 9, 4, 5 }, { 4, 8, 5 }, { 4, 1, 8 },
				{ 8, 1, 10 }, { 8, 10, 3 }, { 5, 8, 3 }, { 5, 3, 2 }, { 2, 3, 7 },
				{ 7, 3, 10 }, { 7, 10, 6 }, { 7, 6, 11 }, { 11, 6, 0 }, { 0, 6, 1 },
				{ 6, 10, 1 }, { 9, 11, 0 }, { 9, 2, 11 }, { 9, 5, 2 }, { 7, 11, 2 } };

			List<Vector3> uniformPositions = new List<Vector3>();
			for (int i = 0; i < 12; ++i)
			{
				uniformPositions.Add(new Vector3(vdata[i, 0], vdata[i, 1], vdata[i, 2]));
				createNormal?.Invoke(vdata[i, 0], vdata[i, 1], vdata[i, 2]);
			}
			for (int i = 0; i < 20; ++i)
			{
				Subdivide(uniformPositions, createID, tindices[i, 0], tindices[i, 1], tindices[i, 2], subdivision, createNormal);
			}

			//scale
			foreach(var pos in uniformPositions)
			{
				var p = pos * radius_;
				createPosition(p.X, p.Y, p.Z);
			}
		}

		private static uint CreateID(List<Vector3> positions, Action<uint> createID, uint id1, uint id2
			, Action<float, float, float> createNormal = null)
		{
			//TODO: could detect if edge already calculated and reuse....
			uint i12 = (uint)positions.Count;
			Vector3 v12 = positions[(int)id1] + positions[(int)id2];
			v12 /= v12.Length();
			createNormal?.Invoke(v12.X, v12.Y, v12.Z);
			positions.Add(v12);
			return i12;
		}

		private static void Subdivide(List<Vector3> positions, Action<uint> createID, uint id1, uint id2, uint id3, uint depth
			, Action<float, float, float> createNormal = null)
		{
			if (0 == depth)
			{
				createID(id1);
				createID(id2);
				createID(id3);
				return;
			}

			uint i12 = CreateID(positions, createID, id1, id2, createNormal);
			uint i23 = CreateID(positions, createID, id2, id3, createNormal);
			uint i31 = CreateID(positions, createID, id3, id1, createNormal);

			Subdivide(positions, createID, id1, i12, i31, depth - 1, createNormal);
			Subdivide(positions, createID, id2, i23, i12, depth - 1, createNormal);
			Subdivide(positions, createID, id3, i31, i23, depth - 1, createNormal);
			Subdivide(positions, createID, i12, i23, i31, depth - 1, createNormal);
		}
	}
}
