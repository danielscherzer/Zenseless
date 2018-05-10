namespace SpaceInvadersMvc
{
	using System;
	using System.IO;
	using Zenseless.Base;
	using Zenseless.HLGL;
	using Zenseless.Sound;

	/// <summary>
	/// All sounds should have the same sampling frequency, otherwise mixing them will throw an exception.
	/// </summary>
	public class Sound : Disposable
	{
		public Sound(IContentLoader contentLoader)
		{
			this.contentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			soundEngine = new AudioPlaybackEngine();
			laser = contentLoader.Load<byte[]>("laser.wav"); //content manager will cache it, but its still faster to keep a direct reference
		}

		public void Background()
		{
			//soundEngine.PlaySound(@"sound\Jamie xx - You've Got the Love.mp3");
		}

		public void DestroyEnemy()
		{
			//var memStream = new MemoryStream(Resourcen.EVAXDaughters);
			//soundEngine.PlaySound(memStream);
		}

		public void Lost()
		{
		}

		public void Shoot()
		{
			var stream = new MemoryStream(laser, false); //TODO: extend soundEngine and contentloader
			soundEngine.PlaySound(stream);//TODO: sound with play, stop
		}

		protected override void DisposeResources()
		{
			soundEngine.Dispose();
		}

		private AudioPlaybackEngine soundEngine;
		private byte[] laser;
		private readonly IContentLoader contentLoader;
	}
}
