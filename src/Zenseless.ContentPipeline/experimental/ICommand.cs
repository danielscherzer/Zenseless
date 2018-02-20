namespace Zenseless.HLGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="IState" />
	public interface ICommand : IState
	{
		/// <summary>
		/// Invokes this instance.
		/// </summary>
		void Invoke();
	}
}
