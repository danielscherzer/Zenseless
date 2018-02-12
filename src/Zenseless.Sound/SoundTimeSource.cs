using Zenseless.Base;
using NAudio.Wave;
using System;

namespace Zenseless.Sound
{
	/// <summary>
	/// Encapsulates the playing and seeking of an audio file (unbuffered). 
	/// Intended for use in multi-media applications.
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	/// <seealso cref="Zenseless.Sound.ITimedMedia" />
	public class SoundTimeSource : Disposable, ITimedMedia
	{
		/// <summary>
		/// Occurs each time the time source is finished with running (length is reached).
		/// </summary>
		public event FinishedHandler TimeFinished;

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundTimeSource"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public SoundTimeSource(string fileName)
		{
			waveOutDevice = new WaveOut();
			audioFileReader = new AudioFileReader(fileName);
			loopingWaveStream = new SoundLoopWaveProvider(audioFileReader)
			{
				EnableLooping = false
			};
			waveOutDevice.Init(loopingWaveStream);
			waveOutDevice.PlaybackStopped += (s, a) => playing = false;
			length = (float)audioFileReader.TotalTime.TotalSeconds;
		}

		/// <summary>
		/// Gets or sets the play length in seconds.
		/// </summary>
		/// <value>
		/// The play length in seconds.
		/// </value>
		/// <exception cref="ArgumentException">NAudioFacade cannot change Length</exception>
		public float Length
		{
			get { return length; }
			set { throw new ArgumentException("NAudioFacade cannot change Length"); }
		}

		/// <summary>
		/// Lopping means that after the time source was running for its length it will
		/// continue to run from the beginning
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is looping; otherwise, <c>false</c>.
		/// </value>
		public bool IsLooping
		{
			get { return loopingWaveStream.EnableLooping; }
			set { loopingWaveStream.EnableLooping = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is running and the position is changing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		public bool IsRunning
		{
			get { return playing; }
			set { playing = value; if (playing) waveOutDevice.Play(); else waveOutDevice.Pause(); }
		}

		/// <summary>
		/// Gets or sets the position in seconds.
		/// </summary>
		/// <value>
		/// The position in seconds.
		/// </value>
		public float Position
		{
			get { return (float)audioFileReader.CurrentTime.TotalSeconds; }
			set
			{
				if (Length < value)
				{
					TimeFinished?.Invoke();
				}
				audioFileReader.CurrentTime = TimeSpan.FromSeconds(value);
			}
		}

		private IWavePlayer waveOutDevice;
		private AudioFileReader audioFileReader;
		private SoundLoopWaveProvider loopingWaveStream;

		private bool playing = false;
		private float length = 10.0f;

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			if (waveOutDevice != null)
			{
				waveOutDevice.Stop();
			}
			if (audioFileReader != null)
			{
				audioFileReader.Dispose();
				audioFileReader = null;
			}
			if (waveOutDevice != null)
			{
				waveOutDevice.Dispose();
				waveOutDevice = null;
			}
		}
	}
}
