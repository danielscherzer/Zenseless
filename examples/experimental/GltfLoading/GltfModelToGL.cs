namespace Example
{
	using glTFLoader;
	using glTFLoader.Schema;
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Text.RegularExpressions;
	using Zenseless.HLGL;
	using Zenseless.OpenGL;

	public class GltfModelToGL
	{
		private readonly Gltf gltf;
		private readonly List<BufferObject> glBuffers;

		public GltfModelToGL(Stream stream)
		{
			gltf = Interface.LoadModel(stream);
			glBuffers = CreateBuffers(gltf);
			//var nodes = from scene in gltf.Scenes select scene.Nodes;
		}

		public Action CreateDrawCommand(IShaderProgram shaderProgram)
		{
			Action drawCommand = null;
			var idVAO = GL.GenVertexArray();
			foreach (var mesh in gltf.Meshes)
			{
				foreach (var primitive in mesh.Primitives)
				{
					var primitiveType = (PrimitiveType)primitive.Mode;
					GL.BindVertexArray(idVAO);
					if (primitive.Indices.HasValue)
					{
						var accessor = gltf.Accessors[primitive.Indices.Value];
						if (accessor.BufferView.HasValue)
						{
							glBuffers[accessor.BufferView.Value].Activate();
							drawCommand = () => GL.DrawElements(primitiveType, accessor.Count, (DrawElementsType)accessor.ComponentType, 0);
						}
					}
					foreach (var attribute in primitive.Attributes)
					{
						var accessor = gltf.Accessors[attribute.Value];
						if (accessor.BufferView.HasValue)
						{
							var uniformName = attribute.Key.ToLowerInvariant();
							var bindingID = shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, uniformName);
							if (-1 != bindingID)
							{
								glBuffers[accessor.BufferView.Value].Activate();
								var bufferView = gltf.BufferViews[accessor.BufferView.Value];
								var elementBytes = bufferView.ByteStride ?? 0;
								GL.VertexAttribPointer(bindingID, accessor.Count, (VertexAttribPointerType)accessor.ComponentType, accessor.Normalized, elementBytes, accessor.ByteOffset);
								GL.EnableVertexAttribArray(bindingID);
							}
						}
					}
					GL.BindVertexArray(0);
					
					return () =>
					{
						GL.BindVertexArray(idVAO);
						drawCommand();
						GL.BindVertexArray(0);
					};
					//{
					//	return () =>
					//	{
					//		GL.BindVertexArray(idVAO);
					//		GL.DrawArrays(primitiveType, 0, 3);
					//		GL.BindVertexArray(0);
					//	};

					//}
				}
			}
			return null;
		}

		private static List<BufferObject> CreateBuffers(Gltf gltf)
		{
			var glBuffers = new List<BufferObject>();
			var byteBuffers = new List<byte[]>();
			foreach (var buffer in gltf.Buffers)
			{
				byteBuffers.Add(ReadBuffer(buffer));
			}
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

		private static byte[] ReadBuffer(glTFLoader.Schema.Buffer buffer)
		{
			var match = Regex.Match(buffer.Uri, @"data:application/octet-stream;base64,(?<data>.+)");
			if (match.Success)
			{
				var base64Data = match.Groups["data"].Value;
				var binData = Convert.FromBase64String(base64Data);
				if (binData.Length != buffer.ByteLength) throw new FormatException("Buffer has wrong length");
				return binData;
			}
			throw new FormatException("Only support inline encoded buffers, not external buffer files.");
		}
	}
}
