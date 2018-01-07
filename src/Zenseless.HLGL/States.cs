using System;
using System.Numerics;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public static partial class States
	{
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="Zenseless.HLGL.IStateBool" />
		public interface IBackfaceCulling : IStateBool { };
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="Zenseless.HLGL.IStateBool" />
		public interface IBlending : IStateBool { };
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="IStateTyped{Vector4}" />
		public interface IClearColor : IStateTyped<Vector4> { };
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="IStateTyped{Single}" />
		public interface ILineWidth : IStateTyped<float> { };
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="IStateBool" />
		public interface IPointSprite : IStateBool { };
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="Zenseless.HLGL.IStateBool" />
		public interface IShaderPointSize : IStateBool { };
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="Zenseless.HLGL.IStateBool" />
		public interface IZBufferTest : IStateBool { };
		//public interface IActiveShader : IStateHandle { };
	}
}
