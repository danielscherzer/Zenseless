using System;
using System.Runtime.InteropServices;

namespace Example
{
	public static class ArrayBuffer
	{
		public static T[] FromByteArray<T>(this byte[] source, int byteOffset, int destinationCount) where T : struct
		{
			T[] destination = new T[destinationCount];
			GCHandle handle = GCHandle.Alloc(destination, GCHandleType.Pinned);
			try
			{
				IntPtr pointer = handle.AddrOfPinnedObject();
				Marshal.Copy(source, byteOffset, pointer, destinationCount * Marshal.SizeOf<T>());
				return destination;
			}
			finally
			{
				if (handle.IsAllocated)
					handle.Free();
			}
		}

		public static float[] ToFloatArray<SRC>(this SRC[] source, int byteOffset = 0) where SRC : struct
		{
			var destination = new float[source.Length * Marshal.SizeOf<SRC>() / 4];
			GCHandle handle = GCHandle.Alloc(source, GCHandleType.Pinned);
			try
			{
				IntPtr pointer = handle.AddrOfPinnedObject();
				Marshal.Copy(pointer, destination, 0, destination.Length);
				return destination;
			}
			finally
			{
				if (handle.IsAllocated)
					handle.Free();
			}
		}
	}
}
