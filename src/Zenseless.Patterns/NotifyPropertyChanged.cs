namespace Zenseless.Patterns
{
	using System;
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

		/// <summary>
		/// Sets a property and Raises the <seealso cref="PropertyChanged"/> event. Shoudl be called from inside the property setter
		/// </summary>
		/// <typeparam name="TYPE">The type of the property.</typeparam>
		/// <param name="valueBackend">The backing variable that contains the property value.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="action">An action that is executed after changing the property, but before invoking the event.</param>
		/// <param name="memberName">Auto filled name of the property.</param>
		protected void SetNotify<TYPE>(ref TYPE valueBackend, TYPE value, Action<TYPE> action = null, [CallerMemberName] string memberName = "")
		{
			valueBackend = value;
			action?.Invoke(value);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
