namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TYPE">The type of the ype.</typeparam>
	/// <seealso cref="Zenseless.HLGL.IState" />
	public interface IStateTyped<TYPE> : IState
	{
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		TYPE Value { get; set; }
	}
}
