namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IRenderState
	{
		/// <summary>
		/// Gets this instance.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <returns></returns>
		TYPE Get<TYPE>() where TYPE : struct, IState;
		/// <summary>
		/// Sets the specified value.
		/// </summary>
		/// <typeparam name="TYPE">The type of the ype.</typeparam>
		/// <param name="value">The value.</param>
		void Set<TYPE>(in TYPE value) where TYPE : struct, IState;
	}
}