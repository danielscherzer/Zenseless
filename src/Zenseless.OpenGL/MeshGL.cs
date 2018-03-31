using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Zenseless.OpenGL
{
	class MeshGL
	{
		public void SetAttribute<DataElement>(string name, DataElement[] data) where DataElement : struct
		{
			var buffer = RequestBuffer(name, BufferTarget.ArrayBuffer);
			buffer.Set(data, BufferUsageHint.StaticDraw);
		}

		private Dictionary<string, BufferObject> attributeBuffer = new Dictionary<string, BufferObject>();

		private BufferObject RequestBuffer(string name, BufferTarget bufferTarget)
		{
			if (!attributeBuffer.TryGetValue(name, out BufferObject buffer))
			{
				buffer = new BufferObject(bufferTarget);
				attributeBuffer[name] = buffer;
			}
			return buffer;
		}
	}
}
