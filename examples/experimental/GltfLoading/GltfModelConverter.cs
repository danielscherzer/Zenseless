using glTFLoader;
using glTFLoader.Schema;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Zenseless.Geometry;

namespace Example
{
	public static class GltfModelConverter
	{
		public static Color4 ExtractBaseColor(this Material material)
		{
			var baseColor = material.PbrMetallicRoughness.BaseColorFactor;
			return new Color4(baseColor[0], baseColor[1], baseColor[2], baseColor[3]);
		}

		public static List<byte[]> ExtractByteBuffers(this Gltf gltf, Func<string, byte[]> externalReferenceSolver)
		{
			var byteBuffers = new List<byte[]>();
			for (int i = 0; i < gltf.Buffers.Length; ++i)
			{
				var buffer = Interface.LoadBinaryBuffer(gltf, i, externalReferenceSolver);
				byteBuffers.Add(buffer);
			}
			return byteBuffers;
		}

		public static Dictionary<int, Transformation> ExtractJointNodeInverseBindTransforms(this Gltf gltf, List<byte[]> byteBuffers)
		{
			var jointNodeInverseBindTransform = new Dictionary<int, Transformation>();
			if (gltf.Skins is null) return jointNodeInverseBindTransform;
			foreach (var skin in gltf.Skins)
			{
				var skinJointNodeIds = skin.Joints;
				var accessor = gltf.Accessors[skin.InverseBindMatrices.Value];
				var view = gltf.BufferViews[accessor.BufferView.Value];
				var inverseBindMatrices = byteBuffers[view.Buffer].FromByteArray<Matrix4x4>(view.ByteOffset + accessor.ByteOffset, accessor.Count);
				var zipped = skinJointNodeIds.Zip(inverseBindMatrices, (key, value) => new { key, value });
				jointNodeInverseBindTransform = zipped.ToDictionary((item) => item.key, (item) => new Transformation(item.value));
			}
			return jointNodeInverseBindTransform;
		}

		public static Transformation ExtractLocalTransform(this glTFLoader.Schema.Node node)
		{
			var translation = Transformation.Translation(node.Translation[0], node.Translation[1], node.Translation[2]);
			var rotation = new Transformation(Matrix4x4.CreateFromQuaternion(new Quaternion(node.Rotation[0], node.Rotation[1], node.Rotation[2], node.Rotation[3])));
			var scale = Transformation.Scale(node.Scale[0], node.Scale[1], node.Scale[2]);
			var m = node.Matrix;
			var transform = new Transformation((new Matrix4x4(m[0], m[1], m[2], m[3], m[4], m[5], m[6], m[7], m[8], m[9], m[10], m[11], m[12], m[13], m[14], m[15])));
			var trs = Transformation.Combine(Transformation.Combine(scale, rotation), translation);
			return Transformation.Combine(trs, transform); //order does not matter because one is identity
		}
	}
}
