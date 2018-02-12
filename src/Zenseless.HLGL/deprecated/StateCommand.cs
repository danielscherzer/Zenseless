using System;

namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TYPE">The type of the ype.</typeparam>
	public class StateCommand<TYPE> : IStateTyped<TYPE> where TYPE : IEquatable<TYPE>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StateCommand{TYPE}"/> class.
		/// </summary>
		/// <param name="command">The gl command.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public StateCommand(Action<TYPE> command, TYPE defaultValue)
		{
			if (command is null) throw new ArgumentNullException();
			this.command = command;
			value = defaultValue;
			Invoke();
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
				Invoke();
			}
		}

		private TYPE value;
		private Action<TYPE> command;

		private void Invoke()
		{
			command.Invoke(value);
		}
	}
}
