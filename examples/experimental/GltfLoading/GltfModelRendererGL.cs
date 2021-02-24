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

		private readonly List<Transformation> localTransforms;
		private readonly Gltf gltf;
		private readonly List<BufferObject> buffersGL;
		private readonly List<ITexture> texturesGL;
		private readonly List<Action> meshDrawCommandsGL = new List<Action>();
		private Dictionary<int, Transformation> jointNodeInverseBindTransform = new Dictionary<int, Transformation>();

		public bool IsSkinned => jointNodeInverseBindTransform.Count > 0;

		private readonly List<Action<float>> animationControllers;

		public GltfModelRendererGL(Stream streamGltf, Func<string, byte[]> externalReferenceSolver
			, Func<string, int> uniformLoc, Func<string, int> attributeLoc)
		{
			gltf = Interface.LoadModel(streamGltf);
			var byteBuffers = gltf.ExtractByteBuffers(externalReferenceSolver);
			localTransforms = CreateLocalTransforms();

			Bounds = CalculateSceneBounds();

			animationControllers = CreateAnimationControllers(gltf, byteBuffers, localTransforms);
			jointNodeInverseBindTransform = gltf.ExtractJointNodeInverseBindTransforms(byteBuffers);

			texturesGL = LoadTextures(gltf, externalReferenceSolver);
			buffersGL = CreateBuffersGL(gltf, byteBuffers);
			UpdateMeshDrawCommandsGL(uniformLoc, attributeLoc);
		}

		private static List<ITexture> LoadTextures(Gltf gltf, Func<string, byte[]> externalReferenceSolver)
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

		internal void UpdateAnimations(float totalSeconds)
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
			Traverse(UpdateWorldMatrix);

			drawCalls();
			GL.BindVertexArray(0);
		}

		public Matrix4x4[] CalculateJointTransforms()
		{
			var jointTransforms = new Dictionary<int, Matrix4x4>();
			void CalcJointTransforms(Transformation globalJointTransformation, int nodeId)
			{
				var node = gltf.Nodes[nodeId];
				if (jointNodeInverseBindTransform.TryGetValue(nodeId, out var inverseBindTransform))
				{
					var jointTransform = Transformation.Combine(inverseBindTransform, globalJointTransformation);
					jointTransforms.Add(nodeId, jointTransform.Matrix);
				}
			}

			Traverse(CalcJointTransforms);

			var joints = new List<Matrix4x4>(jointTransforms.Count);
			foreach(var nodeId in jointNodeInverseBindTransform.Keys)
			{
				joints.Add(jointTransforms[nodeId]);
			}
			return joints.ToArray();
		}

		private void Traverse(Action<Transformation, int> process)
		{
			foreach (var scene in gltf.Scenes)
			{
				TraverseNodes(scene.Nodes, Transformation.Identity, process);
			}
		}

		private List<Transformation> CreateLocalTransforms()
		{
			var nodeTransforms = new List<Transformation>();
			foreach (var node in gltf.Nodes)
			{
				var transformation = node.ExtractLocalTransform();
				nodeTransforms.Add(transformation);
			}
			return nodeTransforms;
		}

		private static List<Action<float>> CreateAnimationControllers(Gltf gltf, List<byte[]> byteBuffers, List<Transformation> localTransforms)
		{
			var animations = gltf.Animations;
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
					var bufferValues = GetBuffer<Vector3>(sampler.Output);
					var node = gltf.Nodes[channel.Target.Node.Value];
					void Interpolator(float time)
					{
						var (lower, upper) = bufferTimes.FindExistingRange(time);
						var weight = time.Normalize(bufferTimes[lower], bufferTimes[upper]);
						var interpolated = Vector3.Lerp(bufferValues[lower], bufferValues[upper], weight);
						node.Translation = interpolated.ToArray();
						localTransforms[channel.Target.Node.Value] = node.ExtractLocalTransform();
						//Debug.WriteLine($"{sampler.Output}: translate {interpolated}");
					}
					animationController.Add(Interpolator);
				}

				void AddAnimationControllerRotation(AnimationChannel channel)
				{
					var sampler = animation.Samplers[channel.Sampler];
					var bufferTimes = GetBuffer<float>(sampler.Input);
					var bufferValues = GetBuffer<Quaternion>(sampler.Output);
					var node = gltf.Nodes[channel.Target.Node.Value];
					void Interpolator(float time)
					{
						var (lower, upper) = bufferTimes.FindExistingRange(time);
						var weight = time.Normalize(bufferTimes[lower], bufferTimes[upper]);
						var interpolated = Quaternion.Lerp(bufferValues[lower], bufferValues[upper], weight);
						node.Rotation = interpolated.ToArray();
						localTransforms[channel.Target.Node.Value] = node.ExtractLocalTransform();
						//Debug.WriteLine($"{sampler.Output}: rotate {interpolated}");
					}
					animationController.Add(Interpolator);
				}

				void AddAnimationControllerScale(AnimationChannel channel)
				{
					var sampler = animation.Samplers[channel.Sampler];
					var bufferTimes = GetBuffer<float>(sampler.Input);
					var node = gltf.Nodes[channel.Target.Node.Value];
					var bufferValues = GetBuffer<Vector3>(sampler.Output);
					void Interpolator(float time)
					{
						var (lower, upper) = bufferTimes.FindExistingRange(time);
						var weight = time.Normalize(bufferTimes[lower], bufferTimes[upper]);
						var interpolated = Vector3.Lerp(bufferValues[lower], bufferValues[upper], weight);
						node.Scale = interpolated.ToArray();
						localTransforms[channel.Target.Node.Value] = node.ExtractLocalTransform();
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

			Traverse(UpdateBounds);
			var size = max - min;
			return new Box3D(min.X, min.Y, min.Z, size.X, size.Y, size.Z);
		}

		public List<Transformation> ExtractCameras()
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
			Traverse(CreateCamera);
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
				var color = primitive.Material.HasValue ? gltf.Materials[primitive.Material.Value].ExtractBaseColor() : Color4.White;
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
