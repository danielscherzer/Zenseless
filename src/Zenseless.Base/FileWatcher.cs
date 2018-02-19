using System.ComponentModel;
using System.IO;

namespace Zenseless.Base
{
	/// <summary>
	/// Watches the file system for changes to a given file
	/// </summary>
	public class FileWatcher : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileWatcher"/> class.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="syncObject">The synchronize object.</param>
		/// <exception cref="FileNotFoundException">File does not exist</exception>
		public FileWatcher(string filePath, ISynchronizeInvoke syncObject = null)
		{
			Dirty = true;
			FullPath = Path.GetFullPath(filePath);
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("File does not exist", filePath);
			}
            watcher = new FileSystemWatcher(Path.GetDirectoryName(FullPath), Path.GetFileName(FullPath))
            {
                SynchronizingObject = syncObject
            };
            watcher.Changed += FileNotification;
			//visual studio does not change a file, but saves a copy and later deletes the original and renames so we watch these too
			watcher.Created += FileNotification;
			watcher.Renamed += FileNotification;
			watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.FileName;
			watcher.EnableRaisingEvents = true;
		}

		/// <summary>
		/// Occurs when the file has changed on disc.
		/// </summary>
		public event FileSystemEventHandler Changed;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="FileWatcher"/> is dirty.
		/// </summary>
		/// <value>
		///   <c>true</c> if dirty; otherwise, <c>false</c>.
		/// </value>
		public bool Dirty { get; set; }

		/// <summary>
		/// Gets the full path of the file.
		/// </summary>
		/// <value>
		/// The full path.
		/// </value>
		public string FullPath { get; private set; }

		private FileSystemWatcher watcher;

		private void FileNotification(object sender, FileSystemEventArgs e)
		{
			Dirty = true;
			Changed?.Invoke(sender, e);
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// Implementers should dispose all their resources her.
		/// </summary>
		protected override void DisposeResources()
		{
			watcher.Dispose();
		}
	}
}
