using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Zenseless.Patterns;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Exception class for Vertex Array O
	/// </summary>
	/// <seealso cref="System.Exception" />
	[Serializable]
	public class VAOException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VAOException" /> class.
		/// </summary>
		/// <param name="msg">The error msg.</param>
		public VAOException(string msg) : base(msg) { }
	}

	/// <summary>
	/// OpenGL Vertex Array Object
	/// </summary>
	/// <seealso cref="Disposable" />
	public class VAO : Disposable
	{
		/// <summary>
		/// Initializes a new OpenGL Vertex Array Object (<see cref="VAO"/>) instance.
		/// </summary>
		public VAO(PrimitiveType primitiveType)
		{
			idVAO = GL.GenVertexArray();
			PrimitiveType = primitiveType;
		}

		/// <summary>
		/// Gets the length of the identifier.
		/// </summary>
		/// <value>
		/// The length of the identifier.
		/// </value>
		public int IDLength { get; private set; } = 0;
		/// <summary>
		/// Gets or sets the type of the primitive.
		/// </summary>
		/// <value>
		/// The type of the primitive.
		/// </value>
		public PrimitiveType PrimitiveType { get; set; }
		/// <summary>
		/// Gets the type of the draw elements.
		/// </summary>
		/// <value>
		/// The type of the draw elements.
		/// </value>
		public DrawElementsType DrawElementsType { get; private set; } = DrawElementsType.UnsignedShort;

		/// <summary>
		/// Sets the index array.
		/// </summary>
		/// <typeparam name="IndexType">The index data type.</typeparam>
		/// <param name="data">The index array data.</param>
		public void SetIndex<IndexType>(IndexType[] data) where IndexType : struct
		{
			if (data is null) return;
			if (0 == data.Length) return;
			Activate();
			var buffer = RequestBuffer(idBufferBinding, BufferTarget.ElementArrayBuffer);
			// set buffer data
			buffer.Set(data, BufferUsageHint.StaticDraw);
			//activate for state
			buffer.Activate();
			//cleanup state
			Deactivate();
			buffer.Deactivate();
			//save data for draw call
			DrawElementsType drawElementsType = GetDrawElementsType(typeof(IndexType));
			IDLength = data.Length;
			DrawElementsType = drawElementsType;
		}

		/// <summary>
		/// Sets a vertex attribute array for the given <paramref name="bindingID"/>.
		/// </summary>
		/// <typeparam name="DataElement">The data element type.</typeparam>
		/// <param name="bindingID">The binding ID.</param>
		/// <param name="data">The attribute array data.</param>
		/// <param name="type">The array elements base type.</param>
		/// <param name="elementSize">Element count for each array element.</param>
		/// <param name="perInstance">
		/// if set to <c>true</c> attribute array contains one entry for each instance
		/// if set to <c>false</c> all attribute array elements are for one instance
		/// </param>
		public void SetAttribute<DataElement>(int bindingID, DataElement[] data, VertexAttribPointerType type, int elementSize, bool perInstance = false) where DataElement : struct
		{
			if (-1 == bindingID) return; //if attribute not used in shader or wrong name
			Activate();
			var buffer = RequestBuffer(bindingID, BufferTarget.ArrayBuffer);
			buffer.Set(data, BufferUsageHint.StaticDraw);
			//activate for state
			buffer.Activate();
			//set data format
			int elementBytes = Marshal.SizeOf(typeof(DataElement));
			GL.VertexAttribPointer(bindingID, elementSize, type, false, elementBytes, 0);
			GL.EnableVertexAttribArray(bindingID);
			if (perInstance)
			{
				GL.VertexAttribDivisor(bindingID, 1);
			}
			else
			{
				lastAttributeLength = data.Length;
			}
			//cleanup state
			Deactivate();
			buffer.Deactivate();
			GL.DisableVertexAttribArray(bindingID);
		}

		/// <summary>
		/// sets or updates a vertex attribute of type Matrix4
		/// Matrix4 is stored row-major, but OpenGL expects data to be column-major, so the Matrix4 inputs become transposed in the shader
		/// </summary>
		/// <param name="bindingID">shader binding location</param>
		/// <param name="data">array of Matrix4 inputs</param>
		/// <param name="perInstance">if set to <c>true</c> [per instance].</param>
		public void SetAttribute(int bindingID, Matrix4x4[] data, bool perInstance = false)
		{
			if (-1 == bindingID) return; //if matrix not used in shader or wrong name
			Activate();
			var buffer = RequestBuffer(bindingID, BufferTarget.ArrayBuffer);
			// set buffer data
			buffer.Set(data, BufferUsageHint.StaticDraw);
			//activate for state
			buffer.Activate();
			//set data format
			int columnBytes = Marshal.SizeOf(typeof(Vector4));
			int elementBytes = Marshal.SizeOf(typeof(Matrix4x4));
			for (int i = 0; i < 4; i++)
			{
				GL.VertexAttribPointer(bindingID + i, 4, VertexAttribPointerType.Float, false, elementBytes, columnBytes * i);
				GL.EnableVertexAttribArray(bindingID + i);
				if (perInstance)
				{
					GL.VertexAttribDivisor(bindingID + i, 1);
				}
			}
			//cleanup state
			Deactivate();
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			for (int i = 0; i < 4; i++)
			{
				GL.DisableVertexAttribArray(bindingID + i);
			}
		}
		
		/// <summary>
		/// Draws the VAO data (instanced if specified).
		/// </summary>
		/// <param name="instanceCount">The instance count (how often should the VAO data be drawn).</param>
		public void Draw(int instanceCount = 1)
		{
			Activate();
			if (0 == IDLength)
			{
				GL.DrawArraysInstanced(PrimitiveType, 0, lastAttributeLength, instanceCount);
			}
			else
			{
				GL.DrawElementsInstanced(PrimitiveType, IDLength, DrawElementsType, (IntPtr)0, instanceCount);
			}
			Deactivate();
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// Implementers should dispose all their resources her.
		/// </summary>
		protected override void DisposeResources()
		{
			foreach (var buffer in boundBuffers.Values)
			{
				buffer.Dispose();
			}
			boundBuffers.Clear();
			GL.DeleteVertexArray(idVAO);
			idVAO = 0;
		}
		
		/// <summary>
				/// The identifier vao
				/// </summary>
		private int idVAO;
		/// <summary>
		/// The identifier buffer binding
		/// </summary>
		private const int idBufferBinding = int.MaxValue;
		/// <summary>
		/// The bound buffers
		/// </summary>
		private Dictionary<int, BufferObject> boundBuffers = new Dictionary<int, BufferObject>();
		private int lastAttributeLength = 0;

		private void Activate()
		{
			GL.BindVertexArray(idVAO);
		}

		private void Deactivate()
		{
			GL.BindVertexArray(0);
		}

		private static DrawElementsType GetDrawElementsType(Type type)
		{
			if (type == typeof(byte)) return DrawElementsType.UnsignedByte;
			if (type == typeof(ushort)) return DrawElementsType.UnsignedShort;
			if (type == typeof(uint)) return DrawElementsType.UnsignedInt;
			throw new ArgumentException($"Invalid index type '{type.FullName}'");
		}

		private BufferObject RequestBuffer(int bindingID, BufferTarget bufferTarget)
		{
			if (!boundBuffers.TryGetValue(bindingID, out BufferObject buffer))
			{
				buffer = new BufferObject(bufferTarget);
				boundBuffers[bindingID] = buffer;
			}
			return buffer;
		}
	}
}
