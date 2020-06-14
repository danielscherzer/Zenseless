using NAudio.Wave;

namespace Zenseless.Sound
{
	/// <summary>
	/// A wave stream that is looped when played back (can be turned off)
	/// </summary>
	/// <seealso cref="IWaveProvider" />
	public class SoundLoopWaveProvider : IWaveProvider
	{
		readonly WaveStream sourceStream;

		/// <summary>
		/// Creates a an adapter around an input WaveStream that enables looping
		/// </summary>
		/// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
		/// or else we will not loop to the start again.</param>
		public SoundLoopWaveProvider(WaveStream sourceStream)
		{
			this.sourceStream = sourceStream;
			this.EnableLooping = true;
		}

		/// <summary>
		/// Use this to turn looping on or off
		/// </summary>
		public bool EnableLooping { get; set; }

		/// <summary>
		/// Return source stream's wave format
		/// </summary>
		public WaveFormat WaveFormat
		{
			get { return sourceStream.WaveFormat; }
		}

		/// <summary>
		/// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>
		/// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
		/// </returns>
		public int Read(byte[] buffer, int offset, int count)
		{
			int totalBytesRead = 0;

			while (totalBytesRead < count)
			{
				int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
				if (bytesRead == 0)
				{
					if (sourceStream.Position == 0 || !EnableLooping)
					{
						// something wrong with the source stream
						break;
					}
					// loop
					sourceStream.Position = 0;
				}
				totalBytesRead += bytesRead;
			}
			return totalBytesRead;
		}
	}
}