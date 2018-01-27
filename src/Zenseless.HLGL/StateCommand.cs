using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TYPE">The type of the ype.</typeparam>
	public class StateCommandGL<TYPE> : IStateTyped<TYPE> where TYPE : IEquatable<TYPE>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StateCommandGL{TYPE}"/> class.
		/// </summary>
		/// <param name="glCommand">The gl command.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public StateCommandGL(Action<TYPE> glCommand, TYPE defaultValue)
		{
			if (glCommand is null) throw new ArgumentNullException();
			this.glCommand = glCommand;
			value = defaultValue;
			UpdateGL();
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public TYPE Value
		{
			get => value;
			set
			{
				if (value.Equals(this.value)) return;
				this.value = value;
				UpdateGL();
			}
		}

		/// <summary>
		/// The value
		/// </summary>
		private TYPE value;
		/// <summary>
		/// The gl command
		/// </summary>
		private Action<TYPE> glCommand;

		/// <summary>
		/// Updates the gl.
		/// </summary>
		private void UpdateGL()
		{
			glCommand.Invoke(value);
		}
	}
}
