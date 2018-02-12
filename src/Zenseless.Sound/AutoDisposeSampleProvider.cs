using NAudio.Wave;
using System;

namespace Zenseless.Sound
{
	class AutoDisposeSampleProvider : ISampleProvider
	{
		private readonly IDisposable disposable;
		private readonly ISampleProvider sampleProvider;
		private bool isDisposed;

		public AutoDisposeSampleProvider(ISampleProvider sampleProvider, IDisposable disposable)
		{
			if (sampleProvider is null) throw new ArgumentNullException();
			this.sampleProvider = sampleProvider;
			this.disposable = disposable;
		}

		public int Read(float[] buffer, int offset, int count)
		{
			if (isDisposed) return 0;
			int read = sampleProvider.Read(buffer, offset, count);
			if (read == 0)
			{
				disposable.Dispose();
				isDisposed = true;
			}
			return read;
		}

		public WaveFormat WaveFormat { get { return sampleProvider.WaveFormat; } }
	}
}
