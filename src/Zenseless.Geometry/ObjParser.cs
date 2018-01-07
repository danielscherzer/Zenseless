using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// 
	/// </summary>
	public class ObjParser
	{
		/// <summary>
		/// 
		/// </summary>
		public class Vertex
		{
			/// <summary>
			/// The identifier normal
			/// </summary>
			public int idNormal;
			/// <summary>
			/// The identifier position
			/// </summary>
			public int idPos;
			/// <summary>
			/// The identifier tex coord
			/// </summary>
			public int idTexCoord;

			/// <summary>
			/// Initializes a new instance of the <see cref="Vertex"/> class.
			/// </summary>
			/// <param name="idPos">The identifier position.</param>
			/// <param name="idTexCoord">The identifier tex coord.</param>
			/// <param name="idNormal">The identifier normal.</param>
			public Vertex(int idPos, int idTexCoord, int idNormal)
			{
				this.idPos = idPos;
				this.idTexCoord = idTexCoord;
				this.idNormal = idNormal;
			}
		}

		/// <summary>
		/// The material file name
		/// </summary>
		public string materialFileName;
		/// <summary>
		/// The position
		/// </summary>
		public List<Vector3> position = new List<Vector3>();
		/// <summary>
		/// The normals
		/// </summary>
		public List<Vector3> normals = new List<Vector3>();
		/// <summary>
		/// The tex coords
		/// </summary>
		public List<Vector2> texCoords = new List<Vector2>();
		/// <summary>
		/// The faces
		/// </summary>
		public List<List<Vertex>> faces = new List<List<Vertex>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjParser"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public ObjParser(byte[] data)
		{
			char[] splitCharacters = new char[] { ' ' };
			string line;
			using (TextReader reader = new StreamReader((new MemoryStream(data))))
			{
				while (!((line = reader.ReadLine()) is null))
				{
					line = line.Trim(splitCharacters);
					line = line.Replace("  ", " ");

					string[] parameters = line.Split(splitCharacters);
					switch (parameters[0])
					{
						case "mtllib": //material lib
							materialFileName = parameters[1];
							break;
						case "p": // Point
							break;

						case "v": // Vertex
							float x = float.Parse(parameters[1], CultureInfo.InvariantCulture);
							float y = float.Parse(parameters[2], CultureInfo.InvariantCulture);
							float z = float.Parse(parameters[3], CultureInfo.InvariantCulture);
							position.Add(new Vector3(x, y, z));
							break;

						case "vt": // TexCoord
							float u = float.Parse(parameters[1], CultureInfo.InvariantCulture);
							float v = float.Parse(parameters[2], CultureInfo.InvariantCulture);
							texCoords.Add(new Vector2(u, v));
							break;

						case "vn": // Normal
							float nx = float.Parse(parameters[1], CultureInfo.InvariantCulture);
							float ny = float.Parse(parameters[2], CultureInfo.InvariantCulture);
							float nz = float.Parse(parameters[3], CultureInfo.InvariantCulture);
							Vector3 n = new Vector3(nx, ny, nz);
							normals.Add(n);
							break;

						case "f":
							//TODO: add face
							var face = new List<Vertex>();
							faces.Add(face);
							for (int i = 1; i < parameters.Length; ++i)
							{
								face.Add(ParseVertex(parameters[i]));
							}
							break;
						case "g": //new group
							break;
						case "usemtl": //set current material
							break;
					}
				}
			}
		}

		/// <summary>
		/// Parses the vertex.
		/// </summary>
		/// <param name="faceParameter_">The face parameter.</param>
		/// <returns></returns>
		private Vertex ParseVertex(string faceParameter_)
		{
			char[] faceParameterSplitter = new char[] { '/' };
			string[] parameters = faceParameter_.Split(faceParameterSplitter);

			int idPos = ParseID(parameters, 0, position.Count);
			int idTexCoord = ParseID(parameters, 1, texCoords.Count);
			int idNormal = ParseID(parameters, 2, normals.Count);
			return new Vertex(idPos, idTexCoord, idNormal);
		}

		/// <summary>
		/// Parses the identifier.
		/// </summary>
		/// <param name="parameters_">The parameters.</param>
		/// <param name="pos_">The position.</param>
		/// <param name="idCount">The identifier count.</param>
		/// <returns></returns>
		private static int ParseID(IList<string> parameters_, int pos_, int idCount)
		{
			if (parameters_.Count > pos_)
			{
				if (int.TryParse(parameters_[pos_], out int index))
				{
					if (index < 0) index = idCount + index;
					else index = index - 1;
					return index;
				}
			}
			return -1;
		}
	}
}
