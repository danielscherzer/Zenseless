namespace Zenseless.Patterns
{
	/// <summary>
	/// Interface for updates at a specified absolute time.
	/// </summary>
	public interface ITimedUpdate
	{
		/// <summary>
		/// Updates at the specified absolute time.
		/// </summary>
		/// <param name="absoluteTime">The absolute time in seconds.</param>
		void Update(float absoluteTime);
	}
}
