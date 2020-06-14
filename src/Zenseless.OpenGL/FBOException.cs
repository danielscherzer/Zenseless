using System;
using System.Runtime.Serialization;

namespace Zenseless.OpenGL
{
	[Serializable]
	internal class FBOException : Exception
	{
		public FBOException()
		{
		}

		public FBOException(string message) : base(message)
		{
		}

		public FBOException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected FBOException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}