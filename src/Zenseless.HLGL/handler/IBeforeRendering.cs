namespace Zenseless.HLGL
{
	/// <summary>
	/// Interface for handlers that need to be called once a frame before rendering
	/// </summary>
	public interface IBeforeRendering
	{
		/// <summary>
		/// Will be called once a frame before rendering. 
		/// </summary>
		void BeforeRendering();
	}
}