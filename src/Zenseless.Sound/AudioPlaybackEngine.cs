using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;
using Zenseless.Patterns;

namespace Zenseless.Sound
{
	/// <summary>
	/// This class allows the playing of multiple sounds at the same time. 
	/// Sounds can be read from streams or files.
	/// All sounds are required to have the same sampling frequency.
	/// </summary>
	public class AudioPlaybackEngine : Disposable
	{
		private readonly IWavePlayer outputDevice;
		private readonly MixingSampleProvider mixer;

		/// <summary>
		/// Create a new instance of the playback engine.
		/// </summary>
		/// <param name="sampleRate">For all input sounds that will be played</param>
		/// <param name="channelCount">Output channel count</param>
		public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
		{
			outputDevice = new WaveOutEvent();
			mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
			mixer.ReadFully = true;
			outputDevice.Init(mixer);
			outputDevice.Play();
		}

		/// <summary>
		/// Plays sound from a given file
		/// </summary>
		/// <param name="fileName">input sound file name (can be a compressed format, like mp3)</param>
		/// <param name="looped">should playback be looped forever</param>
		public void PlaySound(string fileName, bool looped = false)
		{
			var input = new AudioFileReader(fileName);
			if (looped)
			{
				var reader = new SoundLoopWaveProvider(input);
				var sampleChannel = new SampleChannel(reader, false);
				AddMixerInput(sampleChannel);
			}
			else
			{
				AddMixerInput(new AutoDisposeSampleProvider(input, input));
			}
		}

		/// <summary>
		/// Plays sound from a stream; you get unbuffered access if you use a file stream 
		/// and buffered access if you use a memory stream
		/// </summary>
		/// <param name="stream">the input stream that contains the sound (can be compressed, like mp3)</param>
		/// <param name="looped">should playback be looped forever</param>
		public void PlaySound(Stream stream, bool looped = false)
		{
			WaveStream reader = FindCorrectWaveStream(stream);
			if (reader is null) return;
			if (looped)
			{
				var sampleChannel = new SampleChannel(new SoundLoopWaveProvider(reader), false);
				AddMixerInput(sampleChannel);
			}
			else
			{
				var sampleChannel = new SampleChannel(reader, false);
				AddMixerInput(new AutoDisposeSampleProvider(sampleChannel, reader));
			}
		}

		private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
		{
			if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
			{
				return input;
			}
			if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
			{
				return new MonoToStereoSampleProvider(input);
			}
			throw new NotImplementedException("Not yet implemented this channel count conversion");
		}

		private static WaveStream FindCorrectWaveStream(Stream stream)
		{
			try
			{
				WaveStream readerStream = new WaveFileReader(stream);
				if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
					readerStream = new BlockAlignReductionStream(readerStream);
				}
				return readerStream;
			} catch { }
			try
			{
				return new Mp3FileReader(stream);
			}
			catch { }
			try
			{
				return new AiffFileReader(stream);
			}
			catch
			{
				return null;
			}
		}

		private void AddMixerInput(ISampleProvider input)
		{
			mixer.AddMixerInput(ConvertToRightChannelCount(input));
		}

		/// <summary>
		/// Implements disposable pattern object disposal. Here it disposes the output device
		/// </summary>
		protected override void DisposeResources()
		{
			outputDevice.Stop();
			outputDevice.Dispose();
		}
	}
}