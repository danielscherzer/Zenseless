using System;

namespace Zenseless.Base
{
	/// <summary>
	/// Implements the default disposing behavior as recommended by Microsoft.
	/// If you have resources that need disposing, subclass this class.
	/// </summary>
	[Serializable]
	public abstract class Disposable : IDisposable
	{
		/// <summary>
		/// Will be called from the default Dispose method. 
		/// Implementers should dispose all their resources her.
		/// </summary>
		protected abstract void DisposeResources();

		/// <summary>
		/// Dispose status of the instance.
		/// </summary>
		public bool Disposed { get{ return disposed; } }

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposed) return;
			if (disposing)
			{
				DisposeResources();
				disposed = true;
			}
		}

		private bool disposed = false;
	}
}
