namespace Zenseless.Patterns
{
	/// <summary>
	/// Interface for querying absolute and last frame time
	/// results are always in seconds
	/// </summary>
	public interface ITime
	{
		/// <summary>
		/// Gets the time since the last frame.
		/// </summary>
		/// <value>
		/// The delta time in seconds.
		/// </value>
		float DeltaTime { get; }
		/// <summary>
		/// Gets the absolute time since start in seconds.
		/// </summary>
		/// <value>
		/// The absolute time in seconds.
		/// </value>
		float AbsoluteTime { get; }
	}
}