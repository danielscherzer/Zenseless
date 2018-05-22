namespace Zenseless.Patterns
{
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Class that implements a RaisePropertyChanged method for handling property changes
	/// </summary>
	/// <seealso cref="INotifyPropertyChanged" />
	public class NotifyPropertyChanged : INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Name of the property. Will be filled automatically with the caller member name.</param>
		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
