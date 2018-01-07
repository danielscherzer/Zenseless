namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	public interface IStateManager
	{
		/// <summary>
		/// Gets this instance.
		/// </summary>
		/// <typeparam name="INTERFACE">The type of the nterface.</typeparam>
		/// <typeparam name="KEYTYPE">The type of the eytype.</typeparam>
		/// <returns></returns>
		INTERFACE Get<INTERFACE, KEYTYPE>()
			where INTERFACE : class, IState
			where KEYTYPE : IState;
		/// <summary>
		/// Registers the specified state implementation.
		/// </summary>
		/// <typeparam name="INTERFACE">The type of the nterface.</typeparam>
		/// <typeparam name="KEYTYPE">The type of the eytype.</typeparam>
		/// <param name="stateImplementation">The state implementation.</param>
		void Register<INTERFACE, KEYTYPE>(IState stateImplementation)
			where INTERFACE : IState
			where KEYTYPE : INTERFACE;
	}
}