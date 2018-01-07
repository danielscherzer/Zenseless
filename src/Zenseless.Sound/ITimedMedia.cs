using System;

namespace Zenseless.Sound
{
	/// <summary>
	/// Delegate type declaration for the finished handler.
	/// </summary>
	public delegate void FinishedHandler();

	/// <summary>
	/// Interface for a timed media source. Something like an abstract stop watch. 
	/// It is intended to abstract from media, like sound files, or videos
	/// It has a length or running time.
	/// Can be started or stopped and allows seeking and looping.
	/// By default looping is off and it is in stopped state.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface ITimedMedia : IDisposable
	{
		/// <summary>
		/// Gets or sets the current positions time in seconds.
		/// </summary>
		/// <value>
		/// The current positions time in seconds.
		/// </value>
		float Position { get; set; }

		/// <summary>
		/// Lopping means that after the the media was running for its length it will 
		/// continue to run from the beginning
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is looping; otherwise, <c>false</c>.
		/// </value>
		bool IsLooping { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is running and the position is changing.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		bool IsRunning { get; set; }

		/// <summary>
		/// Gets or sets the length in seconds.
		/// </summary>
		/// <value>
		/// The length in seconds.
		/// </value>
		float Length { get; set; }

		/// <summary>
		/// Occurs each time the media is finished with running (length is reached).
		/// </summary>
		event FinishedHandler TimeFinished;
	}
}