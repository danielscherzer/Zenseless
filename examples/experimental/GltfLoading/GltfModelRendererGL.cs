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
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class GltfModelRendererGL
	{

		public Box3D Bounds { get; private set; }

		public IEnumerable<Transformation> Cameras => cameras;

		private readonly List<Transformation> localTransforms;
		private readonly Gltf gltf;
		private readonly List<byte[]> byteBuffers;
		private readonly Color4[] materials;
		private readonly List<BufferObject> buffersGL;
		private readonly List<ITexture> texturesGL;
		private List<Action> meshDrawCommandsGL = new List<Action>();
		private List<Transformation> cameras = new List<Transformation>();
		private Dictionary<int, Transformation> jointNodeInverseBindTransform = new Dictionary<int, Transformation>();

		public bool IsSkinned { get; private set; } = false;

		private int[] skinJointNodeIds;
		private readonly List<Action<float>> animationControllers;

		public GltfModelRendererGL(Stream streamGltf, Func<string, byte[]> externalReferenceSolver
			, Func<string, int> uniformLoc, Func<string, int> attributeLoc)
		{
			gltf = Interface.LoadModel(streamGltf);
			byteBuffers = LoadByteBuffers(gltf, externalReferenceSolver);
			localTransforms = CreateLocalTransforms();
			materials = CreateMaterials(gltf.Materials).ToArray();

			Bounds = CalculateSceneBounds();
			cameras = CreateCameras();

			animationControllers = LoadAnimations(gltf.Animations, byteBuffers);
			LoadSkins(gltf.Skins, byteBuffers);

			texturesGL = LoadTextures(externalReferenceSolver);
			buffersGL = CreateBuffersGL(gltf, byteBuffers);
			UpdateMeshDrawCommandsGL(uniformLoc, attributeLoc);
		}

		private List<ITexture> LoadTextures(Func<string, byte[]> externalReferenceSolver)
		{
			var textures = new List<ITexture>();
			if (gltf.Images is null) return textures;
			if (textures is null) return textures;
			var images = new List<ITexture>();
			foreach (var image in gltf.Images)
			{
				
			}
			return textures;
		}

		internal void UpdateAnimations(float totalSeconds, Action<Matrix4x4[]> setJointMatrix)
		{
			//Debug.Clear();
			//Debug.WriteLine($"time={totalSeconds}");
			//for each animation channel update the respective local transform of the node with current time
			foreach (var animationController in animationControllers)
			{
				animationController(totalSeconds);
			}
		}

		internal void Draw(Action<ITransformation> updateWorldMatrix)
		{
			if (updateWorldMatrix == null)
			{
				throw new ArgumentNullException(nameof(updateWorldMatrix));
			}
			Action drawCalls = null;
			void UpdateWorldMatrix(Transformation worldTransformation, int nodeId)
			{
				var node = gltf.Nodes[nodeId];
				if (node.Mesh.HasValue)
				{
					drawCalls += () =>
					{
						updateWorldMatrix(worldTransformation);
						meshDrawCommandsGL[node.Mesh.Value]();
					};
				}
			}
			foreach (var scene in gltf.Scenes)
			{
				TraverseNodes(scene.Nodes, Transformation.Identity, UpdateWorldMatrix);
			}

			drawCalls();
			GL.BindVertexArray(0);
		}

		public Matrix4x4[] CalculateJointTransforms()
		{
			var jointTransforms = new Dictionary<int, Matrix4x4>();
			void CalcJointTransforms(Transformation worldTransformation, int nodeId)
			{
				var node = gltf.Nodes[nodeId];
				if (jointNodeInverseBindTransform.TryGetValue(nodeId, out var inverseBindTransform))
				{
					var ibt = jointNodeInverseBindTransform[nodeId];
					var jointTransform = Transformation.Combine(ibt, worldTransformation);
					jointTransforms.Add(nodeId, jointTransform.Matrix);
				}
			}

			foreach (var scene in gltf.Scenes)
			{
				TraverseNodes(scene.Nodes, Transformation.Identity, CalcJointTransforms);
			}

			var joints = new List<Matrix4x4>(jointTransforms.Count);
			foreach(var nodeId in skinJointNodeIds)
			{
				joints.Add(jointTransforms[nodeId]);
			}
			return joints.ToArray();
		}

		private List<Transformation> CreateLocalTransforms()
		{
			var nodeTransforms = new List<Transformation>();
			foreach (var node in gltf.Nodes)
			{
				var transformation = CreateLocalTransform(node);
				nodeTransforms.Add(transformation);
			}
			return nodeTransforms;
		}

		private List<Action<float>> LoadAnimations(Animation[] animations, List<byte[]> byteBuffers)
		{
			var animationController = new List<Action<float>>();
			if (animations is null) return animationController;
			if (byteBuffers is null) throw new ArgumentNullException(nameof(byteBuffers));
			var accessorBuffers = new Dictionary<int, Array>();

			TYPE[] GetBuffer<TYPE>(int accessorId) where TYPE : struct
			{
				if(accessorBuffers.TryGetValue(accessorId, out var buf))
				{
					return buf as TYPE[];
				}
				var accessor = gltf.Accessors[accessorId];
				var view = gltf.BufferViews[accessor.BufferView.Value];
				var buffer = byteBuffers[view.Buffer].FromByteArray<TYPE>(view.ByteOffset + accessor.ByteOffset, accessor.Count);
				accessorBuffers.Add(accessorId, buffer);
				return buffer;
			}

			foreach (var animation in animations)
			{
				void AddAnimationControllerTranslate(AnimationChannel channel)
				{
					var sampler = animation.Samplers[channel.Sampler];
					var bufferTimes = GetBuffer<float>(sampler.Input);
					var bufferOutput = GetBuffer<Vector3>(sampler.Output);
					var controlPoints = new ControlPoints<Vector3>(bufferTimes, bufferOutput);
					var node = gltf.Nodes[channel.Target.Node.Value];
					void Interpolator(float t)
					{
						var inter = controlPoints.FindPair(t);
						var interpolated = Vector3.Lerp(inter.Item1, inter.Item2, inter.Item3);
						node.Translation = interpolated.ToArray();
						localTransforms[channel.Target.Node.Value] = CreateLocalTransform(node);
						//Debug.WriteLine($"{sampler.Output}: translate {interpolated}");
					}
					animationController.Add(Interpolator);
				}

				void AddAnimationControllerRotation(AnimationChannel channel)
				{
					var sampler = animation.Samplers[channel.Sampler];
					var bufferTimes = GetBuffer<float>(sampler.Input);
					var bufferOutput = GetBuffer<Quaternion>(sampler.Output);
					var controlPoints = new ControlPoints<Quaternion>(bufferTimes, bufferOutput);
					var node = gltf.Nodes[channel.Target.Node.Value];
					void Interpolator(float t)
					{
						var inter = controlPoints.FindPair(t);
						var interpolated = Quaternion.Lerp(inter.Item1, inter.Item2, inter.Item3);
						node.Rotation = interpolated.ToArray();
						localTransforms[channel.Target.Node.Value] = CreateLocalTransform(node);
						//Debug.WriteLine($"{sampler.Output}: rotate {interpolated}");
					}
					animationController.Add(Interpolator);
				}

				void AddAnimationControllerScale(AnimationChannel channel)
				{
					var sampler = animation.Samplers[channel.Sampler];
					var bufferTimes = GetBuffer<float>(sampler.Input);
					var node = gltf.Nodes[channel.Target.Node.Value];
					var bufferOutput = GetBuffer<Vector3>(sampler.Output);
					var controlPoints = new ControlPoints<Vector3>(bufferTimes, bufferOutput);
					void Interpolator(float t)
					{
						var inter = controlPoints.FindPair(t);
						var interpolated = Vector3.Lerp(inter.Item1, inter.Item2, inter.Item3);
						node.Scale = interpolated.ToArray();
						localTransforms[channel.Target.Node.Value] = CreateLocalTransform(node);
						//Debug.WriteLine($"{sampler.Output}: scale {interpolated}");
					}
					animationController.Add(Interpolator);
				}

				foreach (var channel in animation.Channels)
				{
					if (!channel.Target.Node.HasValue) continue;
					switch(channel.Target.Path)
					{
						case AnimationChannelTarget.PathEnum.rotation:
							AddAnimationControllerRotation(channel);
							break;
						case AnimationChannelTarget.PathEnum.translation:
							AddAnimationControllerTranslate(channel);
							break;
						case AnimationChannelTarget.PathEnum.scale:
							AddAnimationControllerScale(channel);
							break;
						default:
							break;
					}
				}
			}
			return animationController;
		}

		private void LoadSkins(Skin[] skins, List<byte[]> byteBuffers)
		{
			if (skins is null) return;
			foreach(var skin in skins)
			{
				skinJointNodeIds = skin.Joints;
				var accessor = gltf.Accessors[skin.InverseBindMatrices.Value];
				var view = gltf.BufferViews[accessor.BufferView.Value];
				var inverseBindMatrices = byteBuffers[view.Buffer].FromByteArray<Matrix4x4>(view.ByteOffset + accessor.ByteOffset, accessor.Count);
				var zipped = skinJointNodeIds.Zip(inverseBindMatrices, (key, value) => new { key, value });
				jointNodeInverseBindTransform = zipped.ToDictionary((item) => item.key, (item) => new Transformation(item.value));
				IsSkinned = true;
			}
		}

		private Box3D CalculateSceneBounds()
		{
			var min = Vector3.One * float.MaxValue;
			var max = Vector3.One * float.MinValue;
			void UpdateBounds(Transformation worldTransformation, int nodeId)
			{
				var node = gltf.Nodes[nodeId];
				if (node.Mesh.HasValue)
				{
					var mesh = gltf.Meshes[node.Mesh.Value];
					foreach (var primitive in mesh.Primitives)
					{
						if (primitive.Attributes.TryGetValue("POSITION", out var attrPos))
						{
							var accessor = gltf.Accessors[attrPos];
							var primMin = new Vector3(accessor.Min[0], accessor.Min[1], accessor.Min[2]);
							primMin = worldTransformation.Transform(primMin);
							min = Vector3.Min(min, primMin);
							var primMax = new Vector3(accessor.Max[0], accessor.Max[1], accessor.Max[2]);
							primMax = worldTransformation.Transform(primMax);
							max = Vector3.Max(max, primMax);
						}
					}
				}
			}

			foreach (var scene in gltf.Scenes)
			{
				TraverseNodes(scene.Nodes, Transformation.Identity, UpdateBounds);
			}
			var size = max - min;
			return new Box3D(min.X, min.Y, min.Z, size.X, size.Y, size.Z);
		}

		private List<Transformation> CreateCameras()
		{
			var cameras = new List<Transformation>();
			void CreateCamera(Transformation worldTransformation, int nodeId)
			{
				var node = gltf.Nodes[nodeId];
				if (node.Camera.HasValue)
				{
					var camera = gltf.Cameras[node.Camera.Value];
					var invWorldTransform = Transformation.Invert(worldTransformation);
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
			}
			foreach (var scene in gltf.Scenes)
			{
				TraverseNodes(scene.Nodes, Transformation.Identity, CreateCamera);
			}
			return cameras;
		}

		private void TraverseNodes(int[] nodes, in Transformation parentTransformation, Action<Transformation, int> process)
		{
			//TODO: redo this as iterator
			if (nodes is null) return;
			foreach (var nodeId in nodes)
			{
				var localTransform = localTransforms[nodeId];
				var worldTransform = Transformation.Combine(localTransform, parentTransformation);
				process(worldTransform, nodeId);
				var node = gltf.Nodes[nodeId];
				TraverseNodes(node.Children, worldTransform, process);
			}
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

		private static IEnumerable<Color4> CreateMaterials(Material[] materials)
		{
			if (materials is null) yield break;
			foreach (var material in materials)
			{
				var tex = material.PbrMetallicRoughness.BaseColorTexture;
				if(!(tex is null))
				{

				}
				var baseColor = material.PbrMetallicRoughness.BaseColorFactor;
				yield return new Color4(baseColor[0], baseColor[1], baseColor[2], baseColor[3]);
			}
		}

		private static List<byte[]> LoadByteBuffers(Gltf gltf, Func<string, byte[]> externalReferenceSolver)
		{
			var byteBuffers = new List<byte[]>();
			for (int i = 0; i < gltf.Buffers.Length; ++i)
			{
				var buffer = Interface.LoadBinaryBuffer(gltf, i, externalReferenceSolver);
				byteBuffers.Add(buffer);
			}
			return byteBuffers;
		}

		private Action CreateMeshDrawCommandGL(glTFLoader.Schema.Mesh mesh, Func<string, int> uniformLoc, Func<string, int> attributeLoc)
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
			var locColor = uniformLoc("baseColor");
			foreach (var primitive in mesh.Primitives)
			{
				var color = primitive.Material.HasValue ? materials[primitive.Material.Value] : Color4.White;
				action += () => GL.Uniform4(locColor, color);
				action += CreatePrimitiveDrawCallGL(primitive, attributeLoc);
			}
			return action;
		}

		private Action CreatePrimitiveDrawCallGL(MeshPrimitive primitive, Func<string, int> attributeLoc)
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
						buffersGL[accessor.BufferView.Value].Activate();
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
					buffersGL[accessor.BufferView.Value].Activate();
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

		private static List<BufferObject> CreateBuffersGL(Gltf gltf, List<byte[]> byteBuffers)
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

		private void UpdateMeshDrawCommandsGL(Func<string, int> uniformLoc, Func<string, int> attributeLoc)
		{
			meshDrawCommandsGL.Clear();
			foreach (var mesh in gltf.Meshes)
			{
				meshDrawCommandsGL.Add(CreateMeshDrawCommandGL(mesh, uniformLoc, attributeLoc));
			}
			//clean-up state
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}
