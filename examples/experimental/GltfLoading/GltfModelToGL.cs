namespace Example
{
	using glTFLoader;
	using glTFLoader.Schema;
	using OpenTK;
	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices;
	using Zenseless.OpenGL;

	public class GltfModelToGL
	{
		public IEnumerable<Matrix4> Cameras => cameras;

		public Vector3 Min { get; private set; } = Vector3.One * float.MaxValue;
		public Vector3 Max { get; private set; } = Vector3.One * float.MinValue;

		private readonly Gltf gltf;
		private readonly List<BufferObject> glBuffers;
		private readonly Color4[] materials;
		private List<Action> meshDrawCommands = new List<Action>();
		//private readonly Zenseless.Geometry.Node root;
		//private readonly List<Zenseless.Geometry.Node> scenes = new List<Zenseless.Geometry.Node>();
		private List<Matrix4> cameras = new List<Matrix4>();

		public GltfModelToGL(Stream stream, Func<string, byte[]> externalReferenceSolver)
		{
			gltf = Interface.LoadModel(stream);
			var buffers = LoadByteBuffers(gltf, externalReferenceSolver);
			glBuffers = CreateBuffers(gltf, buffers);
			materials = CreateMaterials(gltf.Materials).ToArray();
			LoadAnimations(gltf.Animations);
			//root = new Zenseless.Geometry.Node(null);
			//foreach (var scene in gltf.Scenes)
			//{
			//	Traverse(root, scene.Nodes);
			//}
			CalculateSceneBounds();
		}

		private void LoadAnimations(Animation[] animations)
		{
			if (animations is null) return;
			foreach(var animation in animations)
			{
				foreach(var sampler in animation.Samplers)
				{
					//sampler.Input;
				}
			}
		}

		private void CalculateSceneBounds()
		{
			foreach (var scene in gltf.Scenes)
			{
				Traverse(scene.Nodes, Matrix4.Identity);
			}
		}

		private void Traverse(int[] nodes, Matrix4 transform)
		{
			if (nodes is null) return;
			foreach (var nodeId in nodes)
			{
				var node = gltf.Nodes[nodeId];
				var localTransform = CreateLocalTransform(node);
				var worldTransform = localTransform * transform;
				if (node.Mesh.HasValue)
				{
					var mesh = gltf.Meshes[node.Mesh.Value];
					foreach(var primitive in mesh.Primitives)
					{
						var attrPos = primitive.Attributes["POSITION"];
						var accessor = gltf.Accessors[attrPos];
						var primMin = new Vector3(accessor.Min[0], accessor.Min[1], accessor.Min[2]);
						primMin = Vector3.TransformPosition(primMin, worldTransform);
						Min = Vector3.ComponentMin(Min, primMin);
						var primMax = new Vector3(accessor.Max[0], accessor.Max[1], accessor.Max[2]);
						primMax = Vector3.TransformPosition(primMax, worldTransform);
						Max = Vector3.ComponentMax(Max, primMax);
					}
				}
				Traverse(node.Children, worldTransform);
			}
		}

		public Action CreateTreeDrawCommand(Action<Matrix4> updateWorldMatrix)
		{
			if (updateWorldMatrix == null)
			{
				throw new ArgumentNullException(nameof(updateWorldMatrix));
			}

			Action drawNodes = null;
			foreach (var scene in gltf.Scenes)
			{
				drawNodes += CreateNodesDrawCommand(Matrix4.Identity, scene.Nodes, updateWorldMatrix);
			}
			drawNodes += () => GL.BindVertexArray(0);
			return drawNodes;
		}

		private Action CreateNodesDrawCommand(Matrix4 transform, int[] nodes, Action<Matrix4> updateWorldMatrix)
		{
			Action drawCommand = null;
			if (nodes is null) return drawCommand;
			foreach (var nodeId in nodes)
			{
				var node = gltf.Nodes[nodeId];
				var localTransform = CreateLocalTransform(node);
				var worldTransform = localTransform * transform;
				if(node.Camera.HasValue)
				{
					var camera = gltf.Cameras[node.Camera.Value];
					if (camera.Orthographic != null)
					{
						var o = camera.Orthographic;
						cameras.Add(worldTransform.Inverted() * Matrix4.CreateOrthographic(2f * o.Xmag, 2f * o.Ymag, o.Znear, o.Zfar));
					}
					if (camera.Perspective != null)
					{
						var p = camera.Perspective;
						var perspective = Matrix4.CreatePerspectiveFieldOfView(p.Yfov, p.AspectRatio ?? 1f, p.Znear, p.Zfar ?? 1e6f);
						cameras.Add(worldTransform.Inverted() * perspective);
					}
				}
				if (node.Mesh.HasValue)
				{
					drawCommand += () => updateWorldMatrix(worldTransform);
					drawCommand += meshDrawCommands[node.Mesh.Value];
				}
				drawCommand += CreateNodesDrawCommand(worldTransform, node.Children, updateWorldMatrix);
			}
			return drawCommand;
		}

		private static Matrix4 CreateLocalTransform(Node node)
		{
			var translation = Matrix4.CreateTranslation(node.Translation[0], node.Translation[1], node.Translation[2]);
			var rotation = Matrix4.CreateFromQuaternion(new Quaternion(node.Rotation[0], node.Rotation[1], node.Rotation[2], node.Rotation[3]));
			var scale = Matrix4.CreateScale(node.Scale[0], node.Scale[1], node.Scale[2]);
			var m = node.Matrix;
			var matrix = new Matrix4(m[0], m[1], m[2], m[3], m[4], m[5], m[6], m[7], m[8], m[9], m[10], m[11], m[12], m[13], m[14], m[15]);
			var trs = scale * rotation * translation;
			return matrix * trs; //order does not matter because one is identity
		}

		private void Traverse(Zenseless.Geometry.Node parent, int[] nodes)
		{
			if (nodes == null) return;
			foreach (var nodeId in nodes)
			{
				var node = gltf.Nodes[nodeId];
				var m = node.Matrix;
				//var n = new Zenseless.Geometry.Node(new Transformation(new Matrix4x4(m[0], m[1], m[2], m[3], m[4], m[5], m[6]
				//	, m[7], m[8], m[9], m[10], m[11], m[12], m[13], m[14], m[15])), parent);
				if (node.Mesh != null)
				{
					//n.RegisterTypeInstance(node.Mesh);
				}
				//Traverse(n, node.Children);
			}
		}

		public void UpdateDrawCommands(Func<string, int> uniformLoc, Func<string, int> attributeLoc)
		{
			meshDrawCommands.Clear();
			foreach (var mesh in gltf.Meshes)
			{
				meshDrawCommands.Add(CreateMeshDrawCommand(mesh, uniformLoc, attributeLoc));
			}
			//clean-up state
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		private IEnumerable<Color4> CreateMaterials(Material[] materials)
		{
			if (materials is null) yield break;
			foreach (var material in materials)
			{
				var baseColor = material.PbrMetallicRoughness.BaseColorFactor;
				yield return new Color4(baseColor[0], baseColor[1], baseColor[2], baseColor[3]);
			}
		}

		private Action CreateMeshDrawCommand(Mesh mesh, Func<string, int> uniformLoc, Func<string, int> attributeLoc)
		{
			if (uniformLoc == null)
			{
				throw new ArgumentNullException(nameof(uniformLoc));
			}

			if (attributeLoc == null)
			{
				throw new ArgumentNullException(nameof(attributeLoc));
			}

			Action action = null;
			var locColor = uniformLoc("color");
			foreach (var primitive in mesh.Primitives)
			{
				var color = primitive.Material.HasValue ? materials[primitive.Material.Value] : Color4.White;
				action += () => GL.Uniform4(locColor, color);
				action += CreatePrimitiveDrawCall(primitive, attributeLoc);
			}
			return action;
		}

		private Action CreatePrimitiveDrawCall(MeshPrimitive primitive, Func<string, int> attributeLoc)
		{
			if (attributeLoc == null)
			{
				throw new ArgumentNullException(nameof(attributeLoc));
			}

			var idVAO = GL.GenVertexArray();
			var primitiveType = (PrimitiveType)primitive.Mode;
			GL.BindVertexArray(idVAO);
			var vertexCount = 0;
			foreach (var attribute in primitive.Attributes)
			{
				var accessor = gltf.Accessors[attribute.Value];
				if (accessor.BufferView.HasValue)
				{
					var bindingID = attributeLoc(attribute.Key);
					if (-1 != bindingID)
					{
						vertexCount = accessor.Count;
						glBuffers[accessor.BufferView.Value].Activate();
						var bufferView = gltf.BufferViews[accessor.BufferView.Value];
						var elementBytes = bufferView.ByteStride ?? 0;
						var baseTypeCount = mappingTypeToBaseTypeCount[accessor.Type];
						GL.VertexAttribPointer(bindingID, baseTypeCount, (VertexAttribPointerType)accessor.ComponentType, accessor.Normalized, elementBytes, accessor.ByteOffset);
						GL.EnableVertexAttribArray(bindingID);
					}
				}
			}

			if (primitive.Indices.HasValue)
			{
				var accessor = gltf.Accessors[primitive.Indices.Value];
				if (accessor.BufferView.HasValue)
				{
					glBuffers[accessor.BufferView.Value].Activate();
					return () =>
					{
						GL.BindVertexArray(idVAO);
						GL.DrawElements(primitiveType, accessor.Count, (DrawElementsType)accessor.ComponentType, accessor.ByteOffset);
					};
				}
			}
			return () =>
			{
				GL.BindVertexArray(idVAO);
				GL.DrawArrays(primitiveType, 0, vertexCount);
			};
		}

		private static readonly ReadOnlyDictionary<Accessor.TypeEnum, int> mappingTypeToBaseTypeCount =
			new ReadOnlyDictionary<Accessor.TypeEnum, int>(new Dictionary<Accessor.TypeEnum, int>()
			{
				[Accessor.TypeEnum.SCALAR] = 1,
				[Accessor.TypeEnum.VEC2] = 2,
				[Accessor.TypeEnum.VEC3] = 3,
				[Accessor.TypeEnum.VEC4] = 4,
				[Accessor.TypeEnum.MAT2] = 4,
				[Accessor.TypeEnum.MAT3] = 9,
				[Accessor.TypeEnum.MAT4] = 16,
			});

		private static List<BufferObject> CreateBuffers(Gltf gltf, List<byte[]> byteBuffers)
		{
			var glBuffers = new List<BufferObject>();
			foreach (var bufferView in gltf.BufferViews)
			{
				if (bufferView.Target.HasValue)
				{
					var bufferGL = new BufferObject((BufferTarget)bufferView.Target);
					var pinnedArray = GCHandle.Alloc(byteBuffers[bufferView.Buffer], GCHandleType.Pinned);
					var intPtr = pinnedArray.AddrOfPinnedObject() + bufferView.ByteOffset;
					bufferGL.Set(intPtr, bufferView.ByteLength, BufferUsageHint.StaticDraw);
					pinnedArray.Free();
					glBuffers.Add(bufferGL);
				}
				else
				{
					glBuffers.Add(null);
				}
			}
			return glBuffers;
		}

		private static List<byte[]> LoadByteBuffers(Gltf gltf, Func<string, byte[]> externalReferenceSolver)
		{
			var byteBuffers = new List<byte[]>();
			for(int i = 0; i < gltf.Buffers.Length; ++i)
			{
				var buffer = Interface.LoadBinaryBuffer(gltf, i, externalReferenceSolver);
				byteBuffers.Add(buffer);
			}
			return byteBuffers;
		}
	}
}
