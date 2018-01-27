namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TYPE">The type of the ype.</typeparam>
	/// <seealso cref="IState" />
	public interface ICreator<TYPE> : IState
	{
		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		TypedHandle<TYPE> Create();
	}
}
