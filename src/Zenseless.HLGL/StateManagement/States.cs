namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDepthState { };
	/// <summary>
	/// 
	/// </summary>
	public interface IBackfaceCullingState { };
	/// <summary>
	/// 
	/// </summary>
	public interface ILineSmoothState { };
	/// <summary>
	/// 
	/// </summary>
	public interface IPointSpriteState { };
	/// <summary>
	/// 
	/// </summary>
	public interface IShaderPointSizeState { };

	/// <summary>
	/// 
	/// </summary>
	public static partial class States
	{
		/// <summary>
		/// 
		/// </summary>
		/// <seealso cref="IStateTyped{Single}" />
		public interface ILineWidth : IStateTyped<float> { };
	}
}
