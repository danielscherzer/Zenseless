namespace Zenseless.Patterns
{
	using System;
	using System.Collections.Generic;
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
		/// Should be called from inside the property setter.
		/// </summary>
		/// <param name="memberName">Name of the property. Will be filled automatically with the caller member name.</param>
		protected void RaisePropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		/// <summary>
		/// Sets a property if the value has changed and raises the <seealso cref="PropertyChanged"/> event. 
		/// Should be called from inside the property setter.
		/// </summary>
		/// <typeparam name="TYPE">The type of the property.</typeparam>
		/// <param name="valueBackend">The backing variable that contains the property value.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="memberName">Auto filled name of the property.</param>
		protected void SetNotify<TYPE>(ref TYPE valueBackend, TYPE value, [CallerMemberName] string memberName = "")
		{
			if (EqualityComparer<TYPE>.Default.Equals(valueBackend, value)) return;
			valueBackend = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		/// <summary>
		/// Sets a property if the value has changed and calls the given action and raises the <seealso cref="PropertyChanged"/> event. 
		/// Should be called from inside the property setter.
		/// </summary>
		/// <typeparam name="TYPE">The type of the property.</typeparam>
		/// <param name="valueBackend">The backing variable that contains the property value.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="action">An action that is executed after changing the property, but before invoking the event.</param>
		/// <param name="memberName">Auto filled name of the property.</param>
		protected void SetNotify<TYPE>(ref TYPE valueBackend, TYPE value, Action<TYPE> action, [CallerMemberName] string memberName = "")
		{
			if (EqualityComparer<TYPE>.Default.Equals(valueBackend, value)) return;
			valueBackend = value;
			action?.Invoke(value);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
