namespace Zenseless.HLGL
{
	/// <summary>
	/// Interface for handlers that need to be called once a frame after rendering
	/// </summary>
	public interface IAfterRendering
	{
		/// <summary>
		/// Will be called once a frame after rendering, but before the buffer swap. 
		/// </summary>
		void AfterRendering();
	}
}