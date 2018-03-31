using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using Zenseless.Base;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Disposable" />
	public class BufferObject : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BufferObject"/> class.
		/// </summary>
		/// <param name="bufferTarget">The buffer target.</param>
		public BufferObject(BufferTarget bufferTarget)
		{
			BufferTarget = bufferTarget;
			switch(bufferTarget)
			{
				case BufferTarget.ArrayBuffer: Type = ShaderResourceType.Attribute; break;
				case BufferTarget.UniformBuffer: Type = ShaderResourceType.UniformBuffer; break;
				case BufferTarget.ShaderStorageBuffer: Type = ShaderResourceType.RWBuffer; break;
			}
			GL.GenBuffers​(1, out bufferID);
		}

		/// <summary>
		/// Gets the buffer target.
		/// </summary>
		/// <value>
		/// The buffer target.
		/// </value>
		public BufferTarget BufferTarget { get; private set; }
		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public ShaderResourceType Type { get; private set; }

		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			GL.BindBuffer​(BufferTarget, bufferID);
		}

		/// <summary>
		/// Activates the bind.
		/// </summary>
		/// <param name="index">The index.</param>
		public void ActivateBind(int index) //todo: more than one bound buffer is not working, but have different indices; test: glUniformBlockBinding
		{
			Activate();
			BufferRangeTarget target = (BufferRangeTarget)BufferTarget;
			GL.BindBufferBase​(target, index, bufferID);
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.BindBuffer​(BufferTarget, 0);
		}

		/// <summary>
		/// Sets the specified data.
		/// </summary>
		/// <typeparam name="DATA_ELEMENT_TYPE">The type of the ata element type.</typeparam>
		/// <param name="data">The data.</param>
		/// <param name="usageHint">The usage hint.</param>
		public void Set<DATA_ELEMENT_TYPE>(DATA_ELEMENT_TYPE[] data, BufferUsageHint usageHint) where DATA_ELEMENT_TYPE : struct
		{
			Activate();
			int elementBytes = Marshal.SizeOf(typeof(DATA_ELEMENT_TYPE));
			int bufferByteSize = data.Length * elementBytes;
			// set buffer data
			GL.BufferData(BufferTarget, bufferByteSize, data, usageHint);
			//cleanup state
			Deactivate();
		}

		/// <summary>
		/// Sets the specified data.
		/// </summary>
		/// <typeparam name="DATA_TYPE">The type of the ata type.</typeparam>
		/// <param name="data">The data.</param>
		/// <param name="usageHint">The usage hint.</param>
		public void Set<DATA_TYPE>(DATA_TYPE data, BufferUsageHint usageHint) where DATA_TYPE : struct
		{
			Activate();
			var elementBytes = Marshal.SizeOf(typeof(DATA_TYPE));
			// set buffer data
			GL.BufferData(BufferTarget, elementBytes, ref data, usageHint);
			//cleanup state
			Deactivate();
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			if (-1 == bufferID) return;
			GL.DeleteBuffer(bufferID);
			bufferID = -1;
		}

		private int bufferID;
	}
}
