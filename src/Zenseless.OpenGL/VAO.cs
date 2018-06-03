namespace Zenseless.OpenGL
{
	using OpenTK.Graphics.OpenGL4;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Zenseless.Geometry;
	using Zenseless.HLGL;
	using Zenseless.Patterns;

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
	public class VAO : Disposable, IDrawable
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
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			GL.BindVertexArray(idVAO);
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.BindVertexArray(0);
		}

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
		/// Sets the attribute from an array.
		/// </summary>
		/// <param name="bindingID">The binding identifier.</param>
		/// <param name="data">The attribute array data.</param>
		/// <param name="baseType">Each array element consists of a type that is made up of multiple base types like for Vector3 the base type is float.</param>
		/// <param name="baseTypeCount">Each array element consists of a type that is made up of multiple base types like for Vector3 the base type is float and the base type count is 3.</param>
		/// <param name="perInstance">
		/// if set to <c>true</c> attribute array contains one entry for each instance
		/// if set to <c>false</c> all attribute array elements are for one instance
		/// </param>
		public void SetAttribute(int bindingID, Array data, Type baseType, int baseTypeCount, bool perInstance = false)
		{
			if (-1 == bindingID) return; //if attribute not used in shader or wrong name
			UpdateLength(data.Length, perInstance);

			Activate();
			int elementBytes = Marshal.SizeOf(baseType) * baseTypeCount;
			var buffer = RequestBuffer(bindingID, BufferTarget.ArrayBuffer);
			GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				IntPtr pointer = pinnedArray.AddrOfPinnedObject();
				int byteSize = elementBytes * data.Length;
				buffer.Set(pointer, byteSize, BufferUsageHint.StaticDraw);
			}
			finally
			{
				pinnedArray.Free();
			}
			//activate for state
			buffer.Activate();
			//set data format
			GL.VertexAttribPointer(bindingID, baseTypeCount, FindVertexPointerType(baseType), false, elementBytes, 0);
			GL.EnableVertexAttribArray(bindingID);
			if (perInstance)
			{
				GL.VertexAttribDivisor(bindingID, 1);
			}
			//cleanup state
			Deactivate();
			buffer.Deactivate();
			GL.DisableVertexAttribArray(bindingID);
		}

		private void UpdateLength(int dataLength, bool perInstance)
		{
			if (perInstance)
			{
				instanceCount = dataLength;
			}
			else
			{
				attributeLength = dataLength;
			}
		}

		/// <summary>
		/// Sets a vertex attribute array for the given <paramref name="bindingID"/>.
		/// </summary>
		/// <param name="bindingID">The binding ID.</param>
		/// <param name="data">The attribute array data.</param>
		/// <param name="perInstance">
		/// if set to <c>true</c> attribute array contains one entry for each instance
		/// if set to <c>false</c> all attribute array elements are for one instance
		/// </param>
		public void SetAttribute<ELEMENT_TYPE>(int bindingID, ELEMENT_TYPE[] data, bool perInstance = false) where ELEMENT_TYPE : struct
		{
			var type = typeof(ELEMENT_TYPE);
			var baseTypeCount = MeshAttribute.GetBaseTypeCount(type);
			if(baseTypeCount > 1)
			{
				type = typeof(float);
			}
			SetAttribute(bindingID, data, type, baseTypeCount, perInstance);
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
			UpdateLength(data.Length, perInstance);
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
		public void Draw(int instanceCount)
		{
			Activate();
			if (0 == IDLength)
			{
				GL.DrawArraysInstanced(PrimitiveType, 0, attributeLength, instanceCount);
			}
			else
			{
				GL.DrawElementsInstanced(PrimitiveType, IDLength, DrawElementsType, (IntPtr)0, instanceCount);
			}
			Deactivate();
		}

		/// <summary>
		/// Draws the VAO data (instanced if any instance data is given).
		/// </summary>
		public void Draw()
		{
			Activate();
			if (0 == IDLength)
			{
				GL.DrawArraysInstanced(PrimitiveType, 0, attributeLength, instanceCount);
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
		private int attributeLength = 0;
		private int instanceCount = 1;
		private static readonly ReadOnlyDictionary<Type, VertexAttribPointerType> mappingTypeToPointerType =
			new ReadOnlyDictionary<Type, VertexAttribPointerType>(new Dictionary<Type, VertexAttribPointerType>()
			{
				[typeof(sbyte)] = VertexAttribPointerType.Byte,
				[typeof(byte)] = VertexAttribPointerType.UnsignedByte,
				[typeof(ushort)] = VertexAttribPointerType.UnsignedShort,
				[typeof(short)] = VertexAttribPointerType.Short,
				[typeof(uint)] = VertexAttribPointerType.UnsignedInt,
				[typeof(int)] = VertexAttribPointerType.Int,
				[typeof(float)] = VertexAttribPointerType.Float,
			});

		private static readonly ReadOnlyDictionary<Type, DrawElementsType> mappingTypeToDrawElementsType =
			new ReadOnlyDictionary<Type, DrawElementsType>(new Dictionary<Type, DrawElementsType>()
			{
				[typeof(byte)] = DrawElementsType.UnsignedByte,
				[typeof(ushort)] = DrawElementsType.UnsignedShort,
				[typeof(uint)] = DrawElementsType.UnsignedInt,
			});

		private static VertexAttribPointerType FindVertexPointerType(Type type)
		{
			if (mappingTypeToPointerType.TryGetValue(type, out VertexAttribPointerType value))
			{
				return value;
			}
			throw new ArgumentException($"Unrecognized type {type.FullName}.");
		}

		private static DrawElementsType GetDrawElementsType(Type type)
		{
			if(mappingTypeToDrawElementsType.TryGetValue(type, out DrawElementsType drawElement))
			{
				return drawElement;
			}
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
