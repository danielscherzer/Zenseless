namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TYPE">The type of the ype.</typeparam>
	public interface ICreator<TYPE>
	{
		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns></returns>
		TypedHandle<TYPE> Create();
	}
}
