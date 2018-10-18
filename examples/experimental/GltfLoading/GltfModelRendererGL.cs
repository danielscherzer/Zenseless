namespace Example
{
	using glTFLoader;
	using glTFLoader.Schema;
	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Zenseless.Geometry;
	using Zenseless.OpenGL;

	public class GltfModelRendererGL
	{
		public Box3D Bounds { get; private set; }

		public IEnumerable<Transformation> Cameras => cameras;

		private readonly Action draw;
		private readonly Gltf gltf;
		private readonly List<byte[]> byteBuffers;
		private readonly List<BufferObject> glBuffers;
		private readonly Color4[] materials;
		private List<Action> meshDrawCommands = new List<Action>();
		//private readonly Zenseless.Geometry.Node root;
		//private readonly List<Zenseless.Geometry.Node> scenes = new List<Zenseless.Geometry.Node>();
		private List<Transformation> cameras = new List<Transformation>();

		public GltfModelRendererGL(Stream stream, Func<string, byte[]> externalReferenceSolver
			, Func<string, int> uniformLoc, Func<string, int> attributeLoc, Action<ITransformation> updateWorldMatrix)
		{
			gltf = Interface.LoadModel(stream);
			byteBuffers = LoadByteBuffers(gltf, externalReferenceSolver);
			glBuffers = CreateGlBuffers(gltf, byteBuffers);
			materials = CreateMaterials(gltf.Materials).ToArray();
			LoadAnimations(gltf.Animations, byteBuffers);
			//root = new Zenseless.Geometry.Node(null);
			//foreach (var scene in gltf.Scenes)
			//{
			//	Traverse(root, scene.Nodes);
			//}
			var min = Vector3.One * float.MaxValue;
			var max = Vector3.One * float.MinValue;
			CalculateSceneBounds(ref min, ref max);
			var size = max - min;
			Bounds = new Box3D(min.X, min.Y, min.Z, size.X, size.Y, size.Z);
			UpdateDrawCommands(uniformLoc, attributeLoc);
			draw = CreateTreeDrawCommand(updateWorldMatrix);
		}

		private Action CreateTreeDrawCommand(Action<ITransformation> updateWorldMatrix)
		{
			if (updateWorldMatrix == null)
			{
				throw new ArgumentNullException(nameof(updateWorldMatrix));
			}

			Action drawNodes = null;
			foreach (var scene in gltf.Scenes)
			{
				drawNodes += CreateNodesDrawCommand(Transformation.Identity, scene.Nodes, updateWorldMatrix);
			}
			drawNodes += () => GL.BindVertexArray(0);
			return drawNodes;
		}

		private void UpdateDrawCommands(Func<string, int> uniformLoc, Func<string, int> attributeLoc)
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

		internal void Draw(float totalSeconds)
		{
			draw();
		}

		private void LoadAnimations(Animation[] animations, List<byte[]> byteBuffers)
		{
			if (animations is null) return;
			if (byteBuffers == null) throw new ArgumentNullException(nameof(byteBuffers));
			var typedBuffer = new Dictionary<int, Array>();

			TYPE[] GetBuffer<TYPE>(int id) where TYPE : struct
			{
				if(typedBuffer.TryGetValue(id, out var buf))
				{
					return buf as TYPE[];
				}
				var view = gltf.BufferViews[id];
				var buffer = FromByteArray<TYPE>(byteBuffers[view.Buffer], view.ByteOffset, view.ByteLength);
				typedBuffer.Add(id, buffer);
				return buffer;
			}

			foreach (var animation in animations)
			{
				//foreach(var sampler in animation.Samplers)
				//{
				//	var input = gltf.Accessors[sampler.Input];
				//	var output = gltf.Accessors[sampler.Output];
				//	var bufferInput = GetBuffer<float>(input.BufferView.Value);
				//	var bufferOutput = GetBuffer<Vector3>(output.BufferView.Value);
				//}
				foreach(var channel in animation.Channels)
				{
					var sampler = animation.Samplers[channel.Sampler];
					var input = gltf.Accessors[sampler.Input];
					var output = gltf.Accessors[sampler.Output];
					var bufferTimes = GetBuffer<float>(input.BufferView.Value);
					//channel.Target.Node
					switch(channel.Target.Path)
					{
						case AnimationChannelTarget.PathEnum.rotation:
							var bufferOutput = GetBuffer<Vector4>(input.BufferView.Value);
							break;
						case AnimationChannelTarget.PathEnum.translation:
						//case AnimationChannelTarget.PathEnum.scale:
						//	var bufferOutput = GetBuffer<Vector3>(input.BufferView.Value);
						//	break;
						default:
							break;
					}
				}
			}
		}

		private static T[] FromByteArray<T>(byte[] source, int byteOffset, int byteLength) where T : struct
		{
			T[] destination = new T[byteLength / Marshal.SizeOf<T>()];
			GCHandle handle = GCHandle.Alloc(destination, GCHandleType.Pinned);
			try
			{
				IntPtr pointer = handle.AddrOfPinnedObject();
				Marshal.Copy(source, byteOffset, pointer, byteLength);
				return destination;
			}
			finally
			{
				if (handle.IsAllocated)
					handle.Free();
			}
		}

		private void CalculateSceneBounds(ref Vector3 min, ref Vector3 max)
		{
			foreach (var scene in gltf.Scenes)
			{
				Traverse(scene.Nodes, Transformation.Identity, ref min, ref max);
			}
		}

		private void Traverse(int[] nodes, Transformation transform, ref Vector3 min, ref Vector3 max)
		{
			if (nodes is null) return;
			foreach (var nodeId in nodes)
			{
				var node = gltf.Nodes[nodeId];
				var localTransform = CreateLocalTransform(node);
				var worldTransform = Transformation.Combine(localTransform, transform);
				if (node.Mesh.HasValue)
				{
					var mesh = gltf.Meshes[node.Mesh.Value];
					foreach(var primitive in mesh.Primitives)
					{
						if (primitive.Attributes.TryGetValue("POSITION", out var attrPos))
						{
							var accessor = gltf.Accessors[attrPos];
							var primMin = new Vector3(accessor.Min[0], accessor.Min[1], accessor.Min[2]);
							primMin = worldTransform.Transform(primMin);
							min = Vector3.Min(min, primMin);
							var primMax = new Vector3(accessor.Max[0], accessor.Max[1], accessor.Max[2]);
							primMax = worldTransform.Transform(primMax);
							max = Vector3.Max(max, primMax);
						}
					}
				}
				Traverse(node.Children, worldTransform, ref min, ref max);
			}
		}

		private Action CreateNodesDrawCommand(Transformation transform, int[] nodes, Action<ITransformation> updateWorldMatrix)
		{
			Action drawCommand = null;
			if (nodes is null) return drawCommand;
			foreach (var nodeId in nodes)
			{
				var node = gltf.Nodes[nodeId];
				var localTransform = CreateLocalTransform(node);
				var worldTransform = Transformation.Combine(localTransform, transform);
				if(node.Camera.HasValue)
				{
					var camera = gltf.Cameras[node.Camera.Value];
					var invWorldTransform = Transformation.Invert(worldTransform);
					if (camera.Orthographic != null)
					{
						var o = camera.Orthographic;
						var mtx = Matrix4x4.CreateOrthographic(2f * o.Xmag, 2f * o.Ymag, o.Znear, o.Zfar);
						cameras.Add(Transformation.Combine(invWorldTransform, new Transformation(mtx)));
					}
					if (camera.Perspective != null)
					{
						var p = camera.Perspective;
						var mtx = Matrix4x4.CreatePerspectiveFieldOfView(p.Yfov, p.AspectRatio ?? 1f, p.Znear, p.Zfar ?? 1e6f);
						cameras.Add(Transformation.Combine(invWorldTransform, new Transformation(mtx)));
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

		private static Transformation CreateLocalTransform(glTFLoader.Schema.Node node)
		{
			var translation = Transformation.Translation(node.Translation[0], node.Translation[1], node.Translation[2]);
			var rotation = new Transformation(Matrix4x4.CreateFromQuaternion(new Quaternion(node.Rotation[0], node.Rotation[1], node.Rotation[2], node.Rotation[3])));
			var scale = Transformation.Scale(node.Scale[0], node.Scale[1], node.Scale[2]);
			var m = node.Matrix;
			var transform = new Transformation((new Matrix4x4(m[0], m[1], m[2], m[3], m[4], m[5], m[6], m[7], m[8], m[9], m[10], m[11], m[12], m[13], m[14], m[15])));
			var trs = Transformation.Combine(Transformation.Combine(scale, rotation), translation);
			return Transformation.Combine(trs, transform); //order does not matter because one is identity
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


		private IEnumerable<Color4> CreateMaterials(Material[] materials)
		{
			if (materials is null) yield break;
			foreach (var material in materials)
			{
				var baseColor = material.PbrMetallicRoughness.BaseColorFactor;
				yield return new Color4(baseColor[0], baseColor[1], baseColor[2], baseColor[3]);
			}
		}

		private Action CreateMeshDrawCommand(glTFLoader.Schema.Mesh mesh, Func<string, int> uniformLoc, Func<string, int> attributeLoc)
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

		private static List<BufferObject> CreateGlBuffers(Gltf gltf, List<byte[]> byteBuffers)
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
