using System;

namespace Zenseless.Application
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TYPE">The type of the ype.</typeparam>
	public class Reference<TYPE>
	{
		/// <summary>
		/// Occurs when [change].
		/// </summary>
		public event EventHandler<TYPE> Change;

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public TYPE Value
		{
			get { return value; }
			set { this.value = value; Change?.Invoke(this, value); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Reference{TYPE}"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Reference(TYPE value)
		{
			Value = value;
		}

		/// <summary>
		/// The value
		/// </summary>
		protected TYPE value;
	}
}
