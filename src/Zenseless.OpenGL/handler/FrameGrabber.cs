namespace Zenseless.OpenGL
{
	using System.Collections.Generic;
	using System.Drawing;
	using Zenseless.Base;
	using Zenseless.HLGL;

	/// <summary>
	/// Each frame will be saved into a <seealso cref="Bitmap"/>. Can be used to create videos.
	/// </summary>
	/// <seealso cref="Disposable" />
	/// <seealso cref="IAfterRendering" />
	public class FrameGrabber : Disposable, IAfterRendering
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FrameGrabber"/> class.
		/// </summary>
		public FrameGrabber() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FrameGrabber"/> class.
		/// </summary>
		/// <param name="destinationDirectory">The destination directory.</param>
		public FrameGrabber(string destinationDirectory)
		{
			this.DestinationDirectory = destinationDirectory;
		}

		/// <summary>
		/// Will be called once a frame on rendering, but before the buffer swap.
		/// </summary>
		public void AfterRendering()
		{
			screenShots.Add(FrameBuffer.ToBitmap(false));  //no rotate flip for speed
		}

		/// <summary>
		/// Gets the list of captured frames.
		/// </summary>
		/// <value>
		/// The <seealso cref="IReadOnlyList{Bitmap}"/> of frames.
		/// </value>
		public IReadOnlyList<Bitmap> Frames
		{
			get
			{
				if (rotateFlip)
				{
					screenShots.ForEach((bmp) => bmp.RotateFlip()); //rotate flip
					rotateFlip = false;
				}
				return screenShots;
			}
		}

		/// <summary>
		/// Gets the destination directory for saving the captured frames.
		/// </summary>
		/// <value>
		/// The destination directory.
		/// </value>
		public string DestinationDirectory { get; }

		private bool rotateFlip = false;
		private List<Bitmap> screenShots = new List<Bitmap>();

		/// <summary>
		/// Will be called from the default Dispose method.
		/// Implementers should dispose all their resources her.
		/// </summary>
		protected override void DisposeResources()
		{
			if(!string.IsNullOrWhiteSpace(DestinationDirectory))
			{
				Frames.Save(DestinationDirectory);
			}
			screenShots.ForEach((bmp) => bmp.Dispose()); // cleanup
			screenShots.Clear();
		}
	}
}
